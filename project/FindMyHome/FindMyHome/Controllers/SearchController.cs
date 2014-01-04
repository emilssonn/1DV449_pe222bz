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

namespace FindMyHome.Controllers
{
    //[ValidateHttpAntiForgeryTokenAttribute]
    [InitializeSimpleMembership]
    public class SearchController : ApiController
    {
        private IFindMyHomeService _service;

        public SearchController(IFindMyHomeService service)
        {
            this._service = service;
        }

        // GET api/search
        public AdsContainer Get([FromUri]SearchViewModel viewModel, [FromUri]TagSearchViewModel tagSearch)
        {
            try
            {
                var userId = 0;
                if (User.Identity.IsAuthenticated)
                {
                    userId = (int)Membership.GetUser().ProviderUserKey;
                }
                if (viewModel.Paging)
                    viewModel.AdsContainer = this._service.Search(viewModel.SearchTerms, viewModel.ObjectTypes, 
                                                                    viewModel.MaxRent, viewModel.MaxPrice, 
                                                                    viewModel.Offset, viewModel.Limit,
                                                                    userId);
                else
                    viewModel.AdsContainer = this._service.Search(viewModel.SearchTerms, viewModel.ObjectTypes, 
                                                                    viewModel.MaxRent, viewModel.MaxPrice,
                                                                    userId: userId);

                if (viewModel.Ads.Any())
                {
                    
                }


                return viewModel.AdsContainer;

            }
            catch (ExternalDataSourceException e)
            {
                HttpError err = new HttpError(e.Message);
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
