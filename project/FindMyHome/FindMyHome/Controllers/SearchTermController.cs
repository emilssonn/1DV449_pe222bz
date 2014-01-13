using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Exceptions;
using FindMyHome.Filters;
using FindMyHome.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FindMyHome.Controllers
{
    [ValidateHttpAntiForgeryTokenAttribute]
    public class SearchTermController : ApiController
    {
        private IFindMyHomeService _service;

        public SearchTermController(IFindMyHomeService service)
        {
            this._service = service;
        }

		/// <summary>
		/// GET api/searchterm
		/// Return all previous searches made that contains the search term, return only the search terms
		/// </summary>
		/// <param name="terms"></param>
		/// <returns></returns>
        public IEnumerable<string> Get([FromUri]SearchTermViewModel terms)
        {
            try
            {
                return this._service.GetSearchTerms(terms.SearchTerms);
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
