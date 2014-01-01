using FindMyHome.Domain.Entities.Booli;
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

        [Required(ErrorMessage = "A search term is required")]
        public string SearchTerms { get; set; }

        [Range(0, 100)]
        public int? Page { get; set; }

        [Range(1, 100)]
        public int? Size { get; set; }

        //[villa, lägenhet, gård, tomt-mark, fritidshus, parhus,radhus,kedjehus]
        public string ObjectTypes { get; set; }

        #endregion

        public bool Paging
        {
            get
            {
                return this.Page != null && this.Size != null;
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
                if (this._adsContainer != null)
                    throw new Exception("Can only set this once");
                this._adsContainer = value;
            }
        }

        public IList<Ad> Ads 
        {
            get
            {
                return this._adsContainer.Ads.AsReadOnly();
            }
        }

        public int TotalCount 
        {
            get
            {
                return this._adsContainer.TotalCount;
            }
        }

        public int CurrentCount
        {
            get
            {
                return this._adsContainer.CurrentCount;
            }
        }

        public int Limit
        {
            get
            {
                return this._adsContainer.Limit;
            }
        }

        public int Offset
        {
            get
            {
                return this._adsContainer.Offset;
            }
        }

        
    }
}