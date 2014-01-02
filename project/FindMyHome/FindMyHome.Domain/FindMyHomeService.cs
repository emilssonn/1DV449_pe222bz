using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Webservices;
using FindMyHome.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindMyHome.Domain.DAL;

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

        public override AdsContainer Search(string searchTerms, string objectTypes = null, int? offset = 0, int? limit = 30)
        {
            var searchTermsArray = searchTerms.Split(',').Select(s => s.Trim()).ToArray();
            searchTerms = string.Join(",", searchTermsArray);

            if (objectTypes != null)
            {
                var objectTypesArray = objectTypes.Split(',').Select(s => s.Trim()).ToArray();
                objectTypes = string.Join(",", objectTypesArray);
            }

            var adsContainer = this._unitOfWork.AdsContainerRepository
                .Get(a => a.SearchTerms == searchTerms && a.ObjectTypes == (objectTypes == null ? "" : objectTypes))
                .SingleOrDefault();

            if (adsContainer == null)
            {
                var booliWebservice = new BooliWebservice();
                adsContainer = booliWebservice.SearchRaw(searchTerms, objectTypes, offset, limit);

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
                    adsContainer = booliWebservice.SearchRaw(searchTerms, objectTypes, offset, limit);

                    this._unitOfWork.AdsContainerRepository.Insert(adsContainer);
                    adsContainer.NextUpdate = DateTime.Now.AddHours(3);
                    this._unitOfWork.Save();
                }
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
