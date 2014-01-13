using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FindMyHome.Domain.Exceptions;
using FindMyHome.Domain.Webservices;
using FindMyHome.Domain.Entities.Booli;
using System.Web.Mvc;
using FindMyHome.Filters;
using FindMyHome.ViewModels;
using FindMyHome.Domain.Abstract;
using FindMyHome.Domain;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using FindMyHome.Domain.Entities;

namespace FindMyHome.Controllers
{
    [ValidateHttpAntiForgeryTokenAttribute]
    [InitializeSimpleMembership]
    public class SearchController : ApiController
    {
        private IFindMyHomeService _service;

        public SearchController(IFindMyHomeService service)
        {
            this._service = service;
        }

		/// <summary>
		/// GET api/search
		/// Searches for ads from booli and venues from foursquare
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="venue"></param>
		/// <returns></returns>
        public SearchResult Get([FromUri]SearchViewModel viewModel, [FromUri]VenueSearchViewModel venue)
        {
            try
            {
                var userId = 0;
				//Check if user is logged in
                if (User.Identity.IsAuthenticated)
                {
                    userId = (int)Membership.GetUser().ProviderUserKey;
                }
				//The requests contains paging properties
                if (viewModel.Paging)
                    viewModel.AdsContainer = this._service.SearchAds(viewModel.SearchTerms, viewModel.ObjectTypes, 
                                                                    viewModel.MaxRent, viewModel.MaxPrice, 
                                                                    viewModel.Offset, viewModel.Limit,
                                                                    userId);
                else
                    viewModel.AdsContainer = this._service.SearchAds(viewModel.SearchTerms, viewModel.ObjectTypes, 
                                                                    viewModel.MaxRent, viewModel.MaxPrice, 0, 0,
                                                                    userId: userId);

				//If any ads was found, search for venues
                if (viewModel.AdsContainer.Ads.Any() &&
					venue.Venues != null &&
					venue.Venues != string.Empty)
                {
					var venues = this._service.SearchVenues(viewModel.SearchTerms, venue.Venues);
					return new SearchResult(viewModel.AdsContainer, venues.ToList());
                }

				return new SearchResult(viewModel.AdsContainer);
            }
            catch (ExternalDataSourceException e)
            {
				//A Api returned a error
				HttpError err = new HttpError();
				err.Add("Error", e.Message);
				err.Add("DetailedError", e.DetailedMessage);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
			catch (BadRequestException e)
			{
				//Server or API returner badrequest
				HttpError err = new HttpError();
				err.Add("Error", e.Message);
				err.Add("DetailedError", e.DetailedMessage);
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, err));
			}
            catch (Exception e)
            {
				//Something went wrong on the server
				HttpError err = new HttpError();
				err.Add("Error", Properties.Resources.InternalServerErrorSwe);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }    
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            this._service.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}
