using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Helpers
{
	public class EnumHelper
	{
		public static string GetDescriptionOrValue(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());

			DescriptionAttribute attribute
					= Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
						as DescriptionAttribute;

			return attribute == null ? value.ToString() : attribute.Description;
		}

		public static T GetEnumValueFromDescription<T>(string description)
		{
			var type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException();
			FieldInfo[] fields = type.GetFields();
			var field = fields
							.SelectMany(f => f.GetCustomAttributes(
								typeof(DescriptionAttribute), false), (
									f, a) => new { Field = f, Att = a })
							.Where(a => ((DescriptionAttribute)a.Att)
								.Description == description).SingleOrDefault();
			if (field == null)
				throw new ArgumentException();
			else
				return (T)field.Field.GetRawConstantValue();
		}
	}
}
