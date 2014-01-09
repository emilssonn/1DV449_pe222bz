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
        private AdsContainer _adsContainer = null;

        #region From Url

        [Required(
            ErrorMessageResourceType = typeof(Properties.Resources),
            ErrorMessageResourceName = "SearchTermRequired")]
        [MaxLength(
            100,
            ErrorMessageResourceType = typeof(Properties.Resources),
            ErrorMessageResourceName = "SearchTermLength")]
		[MinLength(
			2,
			ErrorMessageResourceType = typeof(Properties.Resources),
            ErrorMessageResourceName = "SearchTermLength")]
        public string SearchTerms { get; set; }

        [Range(0, Int32.MaxValue)]
        public int? Offset { get; set; }

        [Range(1, 30)]
        public int? Limit { get; set; }

        [MaxLength(70)]
		[AdsObjectTypesAttribute]
        public string ObjectTypes { get; set; }

        [Range(0, Int32.MaxValue)]
        public int MaxRent { get; set; }

        [Range(0, Int32.MaxValue)]
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
            get
            {
                return this._adsContainer;
            }
            set
            {
                this._adsContainer = value;
            }
        }

        public IList<Ad> Ads 
        {
            get
            {
                return this._adsContainer.Ads.ToList().AsReadOnly();
            }
        }
 
    }
}