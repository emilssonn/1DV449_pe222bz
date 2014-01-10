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
	public class VenueTermController : ApiController
	{
		private IFindMyHomeService _service;

		public VenueTermController(IFindMyHomeService service)
		{
			this._service = service;
		}

		public IEnumerable<string> Get([FromUri]VenueTermViewModel term)
		{
			try
			{
				return this._service.GetVenueSearchTerms(term.VenueTerm);
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
