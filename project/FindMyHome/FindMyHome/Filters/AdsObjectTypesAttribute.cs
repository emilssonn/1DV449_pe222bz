using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.Filters
{
	public class AdsObjectTypesAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value == null)
				return true;
			try
			{
				string types = value.ToString();
				var objectTypes = types.Split(',');
				foreach (var item in objectTypes)
				{
					var trimed = item.Trim();
					Enum.Parse(typeof(ObjectType), trimed, true);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}