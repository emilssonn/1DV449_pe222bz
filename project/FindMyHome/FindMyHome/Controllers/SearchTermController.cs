using FindMyHome.Domain.Abstract;
using FindMyHome.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FindMyHome.Controllers
{
    public class SearchTermController : ApiController
    {
        private IFindMyHomeService _service;

        public SearchTermController(IFindMyHomeService service)
        {
            this._service = service;
        }

        // GET api/searchterm
        public IEnumerable<string> Get([FromUri]SearchTermViewModel terms)
        {
            try
            {
                return this._service.GetSearchTerms(terms.SearchTerm);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
