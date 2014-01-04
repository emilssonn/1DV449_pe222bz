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
            ErrorMessageResourceName = "SearchTermRequired")]
        [MaxLength(
            100,
            ErrorMessageResourceType = typeof(Properties.Resources),
            ErrorMessageResourceName = "SearchTermLength")]
        public string SearchTerms { get; set; }
    }
}