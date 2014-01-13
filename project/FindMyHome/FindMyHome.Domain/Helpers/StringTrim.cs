using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Helpers
{
	internal static class StringTrim
	{
		/// <summary>
		/// Trims all values of a string seperatly.
		/// Splits the string on the given char.
		/// Trims each value and join the trimmmed values to a new string
		/// </summary>
		/// <param name="stringToTrim"></param>
		/// <param name="splitChar"></param>
		public static string FullTrim(string stringToTrim, char splitChar = ',')
		{
			var arrayToTrim = stringToTrim.Split(splitChar).Select(s => s.Trim()).ToArray();
			stringToTrim = string.Join(",", arrayToTrim);
			return stringToTrim;
		}
	}
}
