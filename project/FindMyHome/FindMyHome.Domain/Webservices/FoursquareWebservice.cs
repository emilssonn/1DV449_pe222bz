using FindMyHome.Domain.Entities.Foursquare;
using FindMyHome.Domain.Exceptions;
using FindMyHome.Domain.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Webservices
{
    internal class FoursquareWebservice
	{
		#region Fields

		private string _foursquareClientId;

        private string _foursquareClientSecret;

        private string _apiVersionDate = "20140101";

		#endregion

		#region Search

		/// <summary>
		/// Searches for venues located near the search term
		/// Searches for venues connected to one or more of the supplied category ids.
		/// </summary>
		/// <param name="searchTerms">Location</param>
		/// <param name="categories">Valid category Ids</param>
		/// <returns></returns>
		public List<Venue> Search(string searchTerms, string categories)
        {
			var rawJson = string.Empty;

			//Configurate the API call
			var authString = String.Format("&client_id={0}&client_secret={1}&v={2}", this._foursquareClientId, this._foursquareClientSecret, this._apiVersionDate);
			var requestUriString = String.Format("https://api.foursquare.com/v2/venues/search/?near={0}&categoryId={1}&intent=browse&limit=50{2}",
				searchTerms, categories, authString);
			var request = (HttpWebRequest)WebRequest.Create(requestUriString);
			request.Method = "GET";
			request.Accept = "application/json";

			try
			{
				//Make the request
				using (var response = request.GetResponse())
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					rawJson = reader.ReadToEnd();
				}
				var jsonRes = JObject.Parse(rawJson);

				//Get the returned venues as Venue objects
				return jsonRes["response"]["venues"].Select(v => new Venue(v)).ToList();
			}
			catch (WebException e)
			{
				//If the webservice returns a Http error

				//Check if the response is a badrequest error and
				//that the errorType is failed_geocode, silently fail, return empty list
				if (e.Status == WebExceptionStatus.ProtocolError &&
					e.Response != null)
				{
					var resp = (HttpWebResponse)e.Response;
					if (resp.StatusCode == HttpStatusCode.BadRequest)
					{
						var body = String.Empty;
						using (var reader = new StreamReader(resp.GetResponseStream()))
						{
							body = reader.ReadToEnd();
						}
						var jsonRes = JObject.Parse(body);
						if ((string)jsonRes["meta"]["errorType"] == "failed_geocode") {
							return new List<Venue>();
						}
					}
				}
				ExceptionHandler.WebException(e, Properties.Resources.FoursquareApiErrorSwe);
				throw;
			}
        }

		#endregion

		#region Categories

		/// <summary>
		/// Fetches all venue categories from foursquare.
		/// </summary>
		/// <returns></returns>
		public List<Category> GetCategories()
        {
            var rawJson = string.Empty;
			//Configurate the API call
            var authString = String.Format("&client_id={0}&client_secret={1}&v={2}", this._foursquareClientId, this._foursquareClientSecret, this._apiVersionDate);
            var requestUriString = String.Format("https://api.foursquare.com/v2/venues/categories/?{0}", authString);
            var request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.Method = "GET";
            request.Accept = "application/json";

            try
            {
				//Make request
                using (var response = request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    rawJson = reader.ReadToEnd();
                }
                var jsonRes = JObject.Parse(rawJson);

				//Get the categories from the response
                return this.ReadCategories(jsonRes["response"]["categories"]);
            }
            catch (WebException e)
            {
				//If the call returns a http error
				ExceptionHandler.WebException(e, Properties.Resources.FoursquareApiErrorSwe);
                throw;
            }
        }

		/// <summary>
		/// Read the categories
		/// Iterate over each category
		/// </summary>
		/// <param name="categoriesJToken"></param>
		/// <returns></returns>
        private List<Category> ReadCategories(JToken categoriesJToken)
        {
            var categories = new List<Category>();
            foreach (var category in categoriesJToken)
            {
                categories.Add(this.ReadCategory(category));
            }
            return categories;
        }

        /// <summary>
        /// A recursive method to add all subcategories to the parent category
		/// The response from foursquare is a list with a few main categories with child categories.
        /// </summary>
        /// <param name="categoryJToken"></param>
        /// <param name="parentCategory"></param>
        /// <returns></returns>
        private Category ReadCategory(JToken categoryJToken, Category parentCategory = null)
        {
            //var cs = new List<Category>();
            var c = new Category(categoryJToken, parentCategory);
            //cs.Add(c);
            if (categoryJToken["categories"] != null && categoryJToken["categories"].Count() > 0)
            {
                foreach (var cat in categoryJToken["categories"])
                {
                    c.SubCategories.Add(this.ReadCategory(cat, c));
                    //cs.AddRange(this.ReadCategory(cat, c));
                }
            }  
            return c;
        }

		#endregion

		public FoursquareWebservice()
        {
            this._foursquareClientId = ConfigurationManager.AppSettings["FoursquareClientId"];
            this._foursquareClientSecret = ConfigurationManager.AppSettings["FoursquareClientSecret"];
        }
    }
}
