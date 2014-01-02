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
    //[ValidateHttpAntiForgeryTokenAttribute]
    public class TagSearchController : ApiController
    {
        private IFindMyHomeService _service;

        public TagSearchController(IFindMyHomeService service)
        {
            this._service = service;
        }

        // GET api/search
        public HttpResponseMessage Get([FromUri]TagSearchViewModel viewModel)
        {
            return null;
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
