using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Entities.Foursquare;
using FindMyHome.Domain.Exceptions;
using FindMyHome.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FindMyHome.Controllers
{
	[Authorize(Roles = "Administrators")]
	[ValidateHttpAntiForgeryTokenAttribute]
    public class CategoriesController : ApiController
    {
        private IFindMyHomeService _service;

        public CategoriesController(IFindMyHomeService service)
        {
            this._service = service;
        }

		/// <summary>
		/// GET api/categories
		/// Refreshes all Foursquare categories
		/// Currently only available from URL
		/// </summary>
		/// <returns></returns>
        public IEnumerable<Category> Get()
        {
            try
            {
                return this._service.RefreshCategories();
            }
			catch (ExternalDataSourceException e)
			{
				HttpError err = new HttpError();
				err.Add("Error", e.Message);
				err.Add("DetailedError", e.DetailedMessage);
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
			}
			catch (BadRequestException e)
			{
				HttpError err = new HttpError();
				err.Add("Error", e.Message);
				err.Add("DetailedError", e.DetailedMessage);
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, err));
			}
			catch (Exception e)
			{
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
