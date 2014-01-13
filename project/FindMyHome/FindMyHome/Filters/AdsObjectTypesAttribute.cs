using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.Filters
{
	public class AdsObjectTypesAttribute : ValidationAttribute
	{
		/// <summary>
		/// Validates the objecttypes string
		/// Splits the string by ',' and validates each value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool IsValid(object value)
		{
			//Object types are not required
			if (value == null)
				return true;
			try
			{
				string types = value.ToString();
				var objectTypes = types.Split(',');
				foreach (var item in objectTypes)
				{
					var trimed = item.Trim();
					try
					{
						//Get the enum value by the description value
						trimed = EnumHelper.GetEnumValueFromDescription<ObjectType>(trimed).ToString();
					}
					catch (ArgumentException)
					{	
						//Do nothing, not all enums have a description value
					}
					//Parse the string to get a enum, if it fails, the validation fails
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