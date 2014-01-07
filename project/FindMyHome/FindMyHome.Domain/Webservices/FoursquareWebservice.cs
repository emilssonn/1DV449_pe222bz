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
        private string _foursquareClientId;

        private string _foursquareClientSecret;

        private string _apiVersionDate = "20140101";

        public List<Venue> Search(string searchTerms, string categories)
        {
			var rawJson = string.Empty;
			var authString = String.Format("&client_id={0}&client_secret={1}&v={2}", this._foursquareClientId, this._foursquareClientSecret, this._apiVersionDate);
			var requestUriString = String.Format("https://api.foursquare.com/v2/venues/search/?near={0}&categoryId={1}&intent=browse&limit=50{2}",
				searchTerms, categories, authString);
			var request = (HttpWebRequest)WebRequest.Create(requestUriString);
			request.Method = "GET";
			request.Accept = "application/json";

			try
			{
				using (var response = request.GetResponse())
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					rawJson = reader.ReadToEnd();
				}
				var jsonRes = JObject.Parse(rawJson);

				return jsonRes["response"]["venues"].Select(v => new Venue(v)).ToList();
			}
			catch (WebException e)
			{
				ExceptionHandler.WebException(e, Properties.Resources.FoursquareApiError);
				throw;
			}
        }

        public List<Category> GetCategories()
        {
            var rawJson = string.Empty;
            var authString = String.Format("&client_id={0}&client_secret={1}&v={2}", this._foursquareClientId, this._foursquareClientSecret, this._apiVersionDate);
            var requestUriString = String.Format("https://api.foursquare.com/v2/venues/categories/?{0}", authString);
            var request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.Method = "GET";
            request.Accept = "application/json";

            try
            {
                using (var response = request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    rawJson = reader.ReadToEnd();
                }
                var jsonRes = JObject.Parse(rawJson);

                return this.ReadCategories(jsonRes["response"]["categories"]);
            }
            catch (WebException e)
            {
				ExceptionHandler.WebException(e, Properties.Resources.FoursquareApiError);
                throw;
            }
        }

        private List<Category> ReadCategories(JToken categoriesJToken)
        {
            var categories = new List<Category>();
            foreach (var category in categoriesJToken)
            {
                categories.Add(this.ReadCategory(category));
            }
            return categories;
        }

        //Recursive
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

        public FoursquareWebservice()
        {
            this._foursquareClientId = ConfigurationManager.AppSettings["FoursquareClientId"];
            this._foursquareClientSecret = ConfigurationManager.AppSettings["FoursquareClientSecret"];
        }
    }
}
