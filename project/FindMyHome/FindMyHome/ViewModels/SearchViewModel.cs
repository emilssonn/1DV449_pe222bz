using FindMyHome.Domain.Entities.Booli;
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
		
		/// <summary>
		/// Required search term that is the location to search for ads in
		/// </summary>
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

		/// <summary>
		/// How many adds should be skipped when fetching the result
		/// </summary>
        [Range(0, Int32.MaxValue,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "OffsetLengthSwe")]
        public int? Offset { get; set; }

		/// <summary>
		/// How many ads should be fetched
		/// </summary>
        [Range(1, 30,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "LimitLengthSwe")]
        public int? Limit { get; set; }

		/// <summary>
		/// Limit the search to specific object types?
		/// </summary>
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

		/// <summary>
		/// Limit the search to a max rent?
		/// </summary>
        [Range(0, Int32.MaxValue,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "MaxRentSwe")]
        public int MaxRent { get; set; }

		/// <summary>
		/// Limit the search to a max price?
		/// </summary>
        [Range(0, Int32.MaxValue,
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "MaxPriceSwe")]
        public int MaxPrice { get; set; }

        #endregion

		/// <summary>
		/// Is the search made using paging?
		/// </summary>
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