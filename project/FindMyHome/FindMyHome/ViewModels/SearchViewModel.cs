﻿using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{

    public class SearchViewModel
    {

        #region From Url

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

        [Range(0, Int32.MaxValue,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "OffsetLengthSwe")]
        public int? Offset { get; set; }

        [Range(1, 30,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "LimitLengthSwe")]
        public int? Limit { get; set; }

        [MaxLength(70,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "ObjectTypesLengthSwe")]
		[MinLength(2,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "ObjectTypesLengthSwe")]
		[AdsObjectTypesAttribute(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "ObjectTypesValueSwe")]
        public string ObjectTypes { get; set; }

        [Range(0, Int32.MaxValue,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "MaxRentSwe")]
        public int MaxRent { get; set; }

        [Range(0, Int32.MaxValue,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "MaxPriceSwe")]
        public int MaxPrice { get; set; }

        #endregion

        public bool Paging
        {
            get
            {
                return this.Offset > 0 && this.Limit > 0;
            }
        }

		public AdsContainer AdsContainer
		{
			get;
			set;
		}
 
    }
}