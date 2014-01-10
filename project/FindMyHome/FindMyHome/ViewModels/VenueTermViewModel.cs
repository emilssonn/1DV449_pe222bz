using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
	public class VenueTermViewModel
	{
		[Required(
		   ErrorMessageResourceType = typeof(Properties.Resources),
		   ErrorMessageResourceName = "SearchTermRequired")]
		[MinLength(
			2,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "SearchTermLength")]
		public string VenueTerm { get; set; }
	}
}