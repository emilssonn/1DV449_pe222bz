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

		/// <summary>
		/// GET api/veneuterm
		/// Return all venue category names that contains the term
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public IEnumerable<string> Get([FromUri]VenueTermViewModel term)
		{
			try
			{
				return this._service.GetVenueSearchTerms(term.VenueTerm);
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
