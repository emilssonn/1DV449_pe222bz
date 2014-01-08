using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Webservices;
using FindMyHome.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindMyHome.Domain.DAL;
using FindMyHome.Domain.Entities;
using FindMyHome.Domain.Entities.Foursquare;
using FindMyHome.Domain.Helpers;

namespace FindMyHome.Domain
{
    public class FindMyHomeService : FindMyHomeServiceBase
    {
        private IUnitOfWork _unitOfWork;

        public FindMyHomeService()
            : this(new UnitOfWork())
        {

        }

        public FindMyHomeService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public override IEnumerable<string> GetSearchTerms(string term)
        {
			term = StringTrim.FullTrim(term);
            var searchTerms = this._unitOfWork.AdsContainerRepository.Get(a => a.SearchTerms.Contains(term));
            return searchTerms
                .Select(a => a.SearchTerms)
                .OrderBy(a => a)
                .ToList();
        }

        public override AdsContainer SearchAds(string searchTerms, string objectTypes = null, int maxRent = 0, int maxPrice = 0, int? offset = 0, int? limit = 30, int userId = 0)
        {
			searchTerms = StringTrim.FullTrim(searchTerms);

            if (objectTypes != null)
            {
				objectTypes = StringTrim.FullTrim(objectTypes);
            }
            
            var adsContainer = this._unitOfWork.AdsContainerRepository
                .Get(a => a.SearchTerms == searchTerms && 
                    (a.ObjectTypes == objectTypes || (a.ObjectTypes == null && objectTypes == null)) && 
                    a.MaxRent == maxRent &&
                    a.MaxPrice == maxPrice)
                .SingleOrDefault();

            if (adsContainer == null)
            {
                var booliWebservice = new BooliWebservice();
                adsContainer = booliWebservice.Search(searchTerms, objectTypes, maxRent, maxPrice, offset, limit);

                this._unitOfWork.AdsContainerRepository.Insert(adsContainer);
                adsContainer.NextUpdate = DateTime.Now.AddHours(3);
                this._unitOfWork.Save();
            }
            else
            {
                if (adsContainer.NextUpdate < DateTime.Now)
                {
                    this._unitOfWork.AdsContainerRepository.Delete(adsContainer);
                    var booliWebservice = new BooliWebservice();
                    adsContainer = booliWebservice.Search(searchTerms, objectTypes, maxRent, maxPrice, offset, limit);

                    this._unitOfWork.AdsContainerRepository.Insert(adsContainer);
                    //adsContainer.NextUpdate = DateTime.Now.AddHours(3);
                    adsContainer.NextUpdate = DateTime.Now.AddMinutes(1);
                    this._unitOfWork.Save();
                }
            }

            if (userId != 0)
            {
                this.SaveUserAdsSearch(userId, adsContainer);
            }

            return adsContainer;
        }

        private void SaveUserAdsSearch(int userId, AdsContainer adsContainer)
        {
            var userAdsSearches = this._unitOfWork.UserAdsSearchRepository
                    .Get(
                        a => a.UserId == userId, 
                        a => a.OrderBy(u => u.SearchTime))
                    .ToList();

            if (userAdsSearches != null)
            {
                var copy = userAdsSearches.SingleOrDefault(a => a.AdsContainerId == adsContainer.Id);
                if (copy != null)
                {
                    this._unitOfWork.UserAdsSearchRepository.Delete(copy);
                }
                else if (userAdsSearches.Count == 10)
                {
                    this._unitOfWork.UserAdsSearchRepository.Delete(userAdsSearches.Last());
                }
            }

            this._unitOfWork.UserAdsSearchRepository.Insert(
                new UserAdsSearch(userId, adsContainer.Id));

            this._unitOfWork.Save();
        }

		public override IEnumerable<Venue> SearchVenues(string searchTerms, string categories)
		{
			searchTerms = StringTrim.FullTrim(searchTerms);
			categories = StringTrim.FullTrim(categories);

			return new FoursquareWebservice().Search(searchTerms, categories);
		}

        public override IEnumerable<Category> RefreshCategories()
        {
            //var foursquareWebservice = new FoursquareWebservice();

            //var newCategories = foursquareWebservice.GetCategories();

			

			
			/*
            foreach (var item in newCategories)
            {
               
            }

            var cToInsert = new List<Category>();
            var cToUpdate = new List<Category>();
            var cToDelete = new List<Category>();
			
            
            newCategories.ForEach(
                c => this._unitOfWork.CategoryRepository.Insert(c));

            this._unitOfWork.Save();
			*/

			//this.SetParentCategory(newCategories);
			var oldCategories = this._unitOfWork.CategoryRepository.Get(c => c.ParentId == null);
			//var test = newCategories.Except(oldCategories).ToList();

			//var lol1 = this.GetBigList(oldCategories);
			//var lol2 = this.GetBigList(newCategories);

			//var test2 = lol2.Except(lol1).ToList();

            return oldCategories;
        }

        private List<Category> CompareCategories(Category nC, List<Category> oldCs)
        {
			var categories = new List<Category>();
			var check = false;
			foreach (var oC in oldCs)
			{
				if (nC.Equals(oC))
					check = true;
				
			}
			if (check)
				categories.Add(nC);
			return categories;
        }

		private IEnumerable<Category> GetBigList(IEnumerable<Category> categories)
		{
			var cs = new List<Category>();
			foreach (var c in categories)
			{
				cs.AddRange(this.GetBigList(c.SubCategories));
				c.SubCategories.Clear();
			}
			cs.AddRange(categories);
			return cs;
		}

		//Recursive
		private void SetParentCategory(List<Category> categories, Category parentCategory = null)
		{
			foreach (var c in categories)
			{
				c.ParentCategory = parentCategory;
				this.SetParentCategory(c.SubCategories, c);
			}
		}

        protected override void Dispose(bool disposing)
        {
            this._unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
