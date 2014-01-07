using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Helpers
{
	internal static class StringTrim
	{
		public static string FullTrim(string stringToTrim)
		{
			var arrayToTrim = stringToTrim.Split(',').Select(s => s.Trim()).ToArray();
			stringToTrim = string.Join(",", arrayToTrim);
			return stringToTrim;
		}
	}
}
