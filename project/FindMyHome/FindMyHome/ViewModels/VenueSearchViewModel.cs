using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
    public class VenueSearchViewModel
    {
        #region From Url

		[MinLength(
			2,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "VenuesLengthSwe")]
		public string Venues { get; set; }

        #endregion
    }
}