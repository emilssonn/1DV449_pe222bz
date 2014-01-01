using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FindMyHome.Domain.Webservices;
using FindMyHome.Domain.Entities.Booli;
using System.Web.Mvc;
using FindMyHome.Filters;
using FindMyHome.ViewModels;
using FindMyHome.Domain.Abstract;
using FindMyHome.Domain;

namespace FindMyHome.Controllers
{
    //[ValidateHttpAntiForgeryTokenAttribute]
    
    public class SearchController : ApiController
    {
        private IFindMyHomeService _service;

        public SearchController(IFindMyHomeService service)
        {
            _service = service;
        }

        // GET api/search
        public IEnumerable<Ad> Get([FromUri]SearchViewModel viewModel)
        {
            try
            {
                if (viewModel.Paging)
                    viewModel.AdsContainer = this._service.Search(viewModel.SearchTerms, viewModel.ObjectTypes, viewModel.Page, viewModel.Size);
                else
                    viewModel.AdsContainer = this._service.Search(viewModel.SearchTerms, viewModel.ObjectTypes);

                if (viewModel.Ads.Any())
                {

                }



                return viewModel.Ads;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
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
