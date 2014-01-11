using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
	public class VenueTermViewModel
	{
		#region From Url

		[Required(
		   ErrorMessageResourceType = typeof(Properties.Resources),
		   ErrorMessageResourceName = "VenueTermRequiredSwe")]
		[MinLength(
			2,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "VenueTermLengthSwe")]
		public string VenueTerm { get; set; }

		#endregion
	}
}