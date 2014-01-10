using FindMyHome.Domain.Entities.Booli;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyHome.Helpers
{
	public static class ObjectTypeExtensions
	{
		public static MvcHtmlString ObjectTypesJSON(this HtmlHelper helper)
		{
			var objectTypes = Enum.GetNames(typeof(ObjectType));
			var obj = new 
				{
					objectTypes = objectTypes
				};
			return new MvcHtmlString(JsonConvert.SerializeObject(obj));
		}
	}
}