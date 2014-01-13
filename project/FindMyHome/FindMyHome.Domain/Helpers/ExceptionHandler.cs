using FindMyHome.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Helpers
{
	internal static class ExceptionHandler
	{
		/// <summary>
		/// Handles a webexception and throws a new exception with some added info
		/// </summary>
		/// <param name="e"></param>
		/// <param name="message"></param>
		public static void WebException(WebException e, string message)
		{
			if (e.Status == WebExceptionStatus.ProtocolError &&
					e.Response != null)
			{
				var resp = (HttpWebResponse)e.Response;
				if (resp.StatusCode == HttpStatusCode.ServiceUnavailable ||
					resp.StatusCode == HttpStatusCode.InternalServerError)
				{
					throw new ExternalDataSourceException(message, ((HttpWebResponse)e.Response).StatusDescription, e);
				}
				else if (resp.StatusCode == HttpStatusCode.BadRequest)
				{
					throw new ExternalDataSourceException(message, Properties.Resources.GenericApiBadReqSwe, e);
				}
			}
			throw e;
		}
	}
}
