using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
    public class SearchTermViewModel
    {
		[Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "SearchTermRequiredSwe")]
		[MaxLength(
			100,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "SearchTermLengthSwe")]
		[MinLength(
			2,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "SearchTermLengthSwe")]
        public string SearchTerms { get; set; }
    }
}