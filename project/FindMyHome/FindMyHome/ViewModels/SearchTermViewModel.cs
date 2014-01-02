using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
    public class SearchTermViewModel
    {
        [Required]
        [MaxLength(100)]
        public string SearchTerm { get; set; }
    }
}