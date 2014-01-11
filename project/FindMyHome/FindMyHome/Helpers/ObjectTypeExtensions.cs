using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Helpers;
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
			var objectTypes = new List<string>();

			foreach (var item in Enum.GetValues(typeof(ObjectType)))
			{
				objectTypes.Add(EnumHelper.GetDescriptionOrValue((ObjectType)item));
			}
			var obj = new 
				{
					ObjectTypes = objectTypes.ToArray()
				};
			return new MvcHtmlString(JsonConvert.SerializeObject(obj));
		}
	}
}