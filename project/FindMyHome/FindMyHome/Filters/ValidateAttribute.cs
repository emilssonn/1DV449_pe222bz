using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace FindMyHome.Filters
{
    public class ValidateAttribute : ActionFilterAttribute
    {
		/// <summary>
		/// Validates the viewmodels before entering the controller
		/// Returns the validation errors as Json and a http error
		/// </summary>
		/// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    actionContext.ModelState);
            }
        }
    }
}