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

        // GET api/search
        public SearchResult Get([FromUri]SearchViewModel viewModel, [FromUri]TagSearchViewModel tagSearch)
        {
            try
            {
                var userId = 0;
                if (User.Identity.IsAuthenticated)
                {
                    userId = (int)Membership.GetUser().ProviderUserKey;
                }
                if (viewModel.Paging)
                    viewModel.AdsContainer = this._service.SearchAds(viewModel.SearchTerms, viewModel.ObjectTypes, 
                                                                    viewModel.MaxRent, viewModel.MaxPrice, 
                                                                    viewModel.Offset, viewModel.Limit,
                                                                    userId);
                else
                    viewModel.AdsContainer = this._service.SearchAds(viewModel.SearchTerms, viewModel.ObjectTypes, 
                                                                    viewModel.MaxRent, viewModel.MaxPrice,
                                                                    userId: userId);

				
                if (viewModel.Ads.Any() &&
					tagSearch.Categories != null &&
					tagSearch.Categories != string.Empty)
                {
					var venues = this._service.SearchVenues(viewModel.SearchTerms, tagSearch.Categories);
					return new SearchResult(viewModel.AdsContainer, venues.ToList());
                }


				return new SearchResult(viewModel.AdsContainer);

            }
            catch (ExternalDataSourceException e)
            {
				HttpError err = new HttpError();
				err.Add("Message", e.Message);
				err.Add("DetailedMessage", e.DetailedMessage);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
			catch (BadRequestException e)
			{
				HttpError err = new HttpError();
				err.Add("Message", e.Message);
				err.Add("DetailedMessage", e.DetailedMessage);
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
			}
            catch (Exception e)
            {
                var message = string.Format(Properties.Resources.InternalServerError);
                HttpError err = new HttpError(message);
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
