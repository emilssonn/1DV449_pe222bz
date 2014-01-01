using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
    public class FoursquareSearchViewModel
    {
        public string SearchTerms { get; set; }

        [Range(0, 100)]
        public int? Page { get; set; }

        [Range(1, 100)]
        public int? Size { get; set; }
    }
}