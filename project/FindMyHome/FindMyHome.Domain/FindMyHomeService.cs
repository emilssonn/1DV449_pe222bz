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
            var searchTerms = this._unitOfWork.AdsContainerRepository.Get(a => a.SearchTerms.Contains(term));
            return searchTerms
                .Select(a => a.SearchTerms)
                .OrderBy(a => a)
                .ToList();
        }

        public override AdsContainer Search(string searchTerms, string objectTypes = null, int maxRent = 0, int maxPrice = 0, int? offset = 0, int? limit = 30, int userId = 0)
        {
            var searchTermsArray = searchTerms.Split(',').Select(s => s.Trim()).ToArray();
            searchTerms = string.Join(",", searchTermsArray);

            if (objectTypes != null)
            {
                var objectTypesArray = objectTypes.Split(',').Select(s => s.Trim()).ToArray();
                objectTypes = string.Join(",", objectTypesArray);
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
                var userAdsSearches = this._unitOfWork.UserAdsSearchRepository.Get(a => a.UserId == userId)
                    .OrderBy(a => a.SearchTime)
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

            return adsContainer;
        }

        

        protected override void Dispose(bool disposing)
        {
            this._unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
