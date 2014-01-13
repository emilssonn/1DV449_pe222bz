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
		/// <summary>
		/// Tries to get the description of enum.
		/// If the description attribute is not there, get the value(string)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetDescriptionOrValue(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());

			DescriptionAttribute attribute
					= Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
						as DescriptionAttribute;

			return attribute == null ? value.ToString() : attribute.Description;
		}

		/// <summary>
		/// Info: Har inte skrivit denna helt själv och har tappat bort källan.
		/// Returns the enum value by mathcing the description attribute to the supplied string
		/// Throws a ArgumentException if the enum with the supplied description is not found
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="description"></param>
		/// <returns></returns>
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
