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
using FindMyHome.Domain.Exceptions;

namespace FindMyHome.Domain
{
    public class FindMyHomeService : FindMyHomeServiceBase
	{
		#region Fields

		private IUnitOfWork _unitOfWork;

		#endregion

		#region Properties

		public FindMyHomeService()
            : this(new UnitOfWork())
        {

        }

        public FindMyHomeService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

		#endregion

		#region Autocomplete

		/// <summary>
		/// Get all venue names with name containing the term.
		/// The names are loaded from the database
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public override IEnumerable<string> GetVenueSearchTerms(string term)
		{
			term = StringTrim.FullTrim(term);
			var searchTerms = this._unitOfWork.CategoryRepository.Get(c => c.DisplayName.Contains(term), c => c.OrderBy(s => s.DisplayName));
			return searchTerms
				.Select(c => c.DisplayName)
				.Distinct()
				.ToList();
		}

		/// <summary>
		/// Get all previous searches containing the term.
		/// Only returns the location part of the search
		/// The searches are loaded from the database
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
        public override IEnumerable<string> GetSearchTerms(string term)
        {
			term = StringTrim.FullTrim(term);
            var searchTerms = this._unitOfWork.AdsContainerRepository.Get(a => a.SearchTerms.Contains(term), a => a.OrderBy(u => u.SearchTerms));
            return searchTerms
                .Select(a => a.SearchTerms)
				.Distinct()
                .ToList();
        }

		#endregion

		#region Ads

		/// <summary>
		/// Searches for ads from booli
		/// Will first check if the search already has been made, the search is cached in the database.
		/// If the search is cached, check if its still valid or enough time as passed to make a new one.
		/// Otherwise, fetch the ads from the API, and cache them in the database.
		/// </summary>
		/// <param name="searchTerms"></param>
		/// <param name="objectTypes"></param>
		/// <param name="maxRent"></param>
		/// <param name="maxPrice"></param>
		/// <param name="offset"></param>
		/// <param name="limit"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public override AdsContainer SearchAds(string searchTerms, string objectTypes = null, int maxRent = 0, int maxPrice = 0, int? offset = 0, int? limit = 30, int userId = 0)
        {
			if (limit == null || limit <= 0)
				limit = 30;
			searchTerms = StringTrim.FullTrim(searchTerms);

            if (objectTypes != null)
            {
				objectTypes = StringTrim.FullTrim(objectTypes);
            }
            
			//Check if the search is cached
            var adsContainer = this._unitOfWork.AdsContainerRepository
                .Get(a => a.SearchTerms == searchTerms && 
                    (a.ObjectTypes == objectTypes || (a.ObjectTypes == null && objectTypes == null)) && 
                    a.MaxRent == maxRent &&
                    a.MaxPrice == maxPrice &&
					a.Limit == limit &&
					a.Offset == offset)
                .SingleOrDefault();

			//Not cached
            if (adsContainer == null)
            {
                var booliWebservice = new BooliWebservice();
                adsContainer = booliWebservice.Search(searchTerms, objectTypes, maxRent, maxPrice, offset, limit);

				//Save in database
				adsContainer.LastUpdate = DateTime.UtcNow;
				adsContainer.NextUpdate = DateTime.UtcNow.AddHours(3);
                this._unitOfWork.AdsContainerRepository.Insert(adsContainer);
                this._unitOfWork.Save();
            }
            else
            {
				//Check if still valid
				if (adsContainer.NextUpdate < DateTime.UtcNow)
                {
                    var booliWebservice = new BooliWebservice();
                    var newAdsContainer = booliWebservice.Search(searchTerms, objectTypes, maxRent, maxPrice, offset, limit);

					//Update the database
					adsContainer.Ads = newAdsContainer.Ads;
					adsContainer.CurrentCount = newAdsContainer.CurrentCount;
					adsContainer.TotalCount = newAdsContainer.TotalCount;
					adsContainer.LastUpdate = DateTime.UtcNow;
					adsContainer.NextUpdate = DateTime.UtcNow.AddHours(3);
                    this._unitOfWork.AdsContainerRepository.Update(adsContainer);
					
                    this._unitOfWork.Save();
                }
            }

			//If a userid was supplied, save the search
            if (userId != 0)
            {
                this.SaveUserAdsSearch(userId, adsContainer);
            }

            return adsContainer;
        }

		/// <summary>
		/// Save the last 10 searches for a user.
		/// This currently does not save the any venues also searched on
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="adsContainer"></param>
        private void SaveUserAdsSearch(int userId, AdsContainer adsContainer)
        {
			//Get any previous searches
            var userAdsSearches = this._unitOfWork.UserAdsSearchRepository
                    .Get(
                        a => a.UserId == userId, 
                        a => a.OrderBy(u => u.SearchTime))
                    .ToList();

			//If found
            if (userAdsSearches != null)
            {
				//Check if a exact copy already exists
                var copy = userAdsSearches.SingleOrDefault(a => a.AdsContainerId == adsContainer.Id);
				//Delete the copy
                if (copy != null)
                {
                    this._unitOfWork.UserAdsSearchRepository.Delete(copy);
                }
                else if (userAdsSearches.Count == 10)
                {
					//If 10 searches already exisits, delete the oldest one
                    this._unitOfWork.UserAdsSearchRepository.Delete(userAdsSearches.Last());
                }
            }

			//Insert the search
            this._unitOfWork.UserAdsSearchRepository.Insert(
                new UserAdsSearch(userId, adsContainer.Id));

            this._unitOfWork.Save();
        }

		/// <summary>
		/// Get the last 10 searches
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public override IEnumerable<string> GetUserSearches(int userId)
		{
			return this._unitOfWork.UserAdsSearchRepository.Get(u => u.UserId == userId).Select(s => s.AdsContainer.SearchTerms).ToList();
		}

		#endregion

		#region Venues

		/// <summary>
		/// Searches for venues on foursquare using the supplied searchterm and categories
		/// Get all the category ids from the db using the names supplied.
		/// Validates that each name as a id else throws a exception.
		/// </summary>
		/// <param name="searchTerms">Location</param>
		/// <param name="categories">Category names</param>
		/// <returns></returns>
		public override IEnumerable<Venue> SearchVenues(string searchTerms, string categories)
		{
			//Trim
			searchTerms = StringTrim.FullTrim(searchTerms);
			categories = StringTrim.FullTrim(categories);

			//Get each seperate category
			var categoriesArray = categories.Split(',');

			//Get the category ids
			var categoriesIds = this._unitOfWork.CategoryRepository.Get(c => categoriesArray.Contains(c.DisplayName))
				.Select(c => c.Id)
				.ToList();
			
			//Check that the same number of categories was found as names supplied.
			//If not, one or more invalid category name was given
			if (categoriesArray.Length != categoriesIds.Count())
			{
				throw new BadRequestException(Properties.Resources.InvalidFoursquareCategoiresHeadSwe, Properties.Resources.InvalidFoursquareCategoiresDescSwe);
			}

			//Join the ids for call to API
			categories = String.Join(",", categoriesIds);

			return new FoursquareWebservice().Search(searchTerms, categories);
		}

		#region Categories

		//Work in progress
		//Todo
		//Refresh all categories, check if any changes have been made
		//Only update displayname if swename is null
		//Admin should be able to get all categories(paged) and then add a swedish translation to each one.

		/// <summary>
		/// Currently deletes all categories in database and inserts the new ones from the foursquare webservice
		/// </summary>
		/// <returns></returns>
        public override IEnumerable<Category> RefreshCategories()
        {
            var foursquareWebservice = new FoursquareWebservice();

            var newCategories = foursquareWebservice.GetCategories();

			var oldCategories = this._unitOfWork.CategoryRepository.Get(c => c.ParentId == null);

			foreach (var oldCat in oldCategories)
			{
				this._unitOfWork.CategoryRepository.Delete(oldCat);
			}

			newCategories.ForEach(
                c => this._unitOfWork.CategoryRepository.Insert(c));

			this._unitOfWork.Save();
			

			//foreach (var item in newCategories)
			//{
               
			//}
			//var cToInsert = new List<Category>();
			//var cToUpdate = new List<Category>();
			//var cToDelete = new List<Category>();
			
            
            //newCategories.ForEach(
                //c => this._unitOfWork.CategoryRepository.Insert(c));

            //this._unitOfWork.Save();

			//this.SetParentCategory(newCategories);
			//var oldCategories = this._unitOfWork.CategoryRepository.Get(c => c.ParentId == null);
			//var test = newCategories.Except(oldCategories).ToList();

			//var lol1 = this.GetBigList(oldCategories);
			//var lol2 = this.GetBigList(newCategories);

			//var test2 = lol2.Except(lol1).ToList();

            return oldCategories;
        }

		//Get all categories as a long list to compare each one by it self
		//Not working

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

		#endregion

		#endregion

		protected override void Dispose(bool disposing)
        {
            this._unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
