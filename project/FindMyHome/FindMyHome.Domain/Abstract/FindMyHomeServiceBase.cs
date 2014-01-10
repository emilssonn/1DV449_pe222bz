using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Entities.Foursquare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Abstract
{
    public abstract class FindMyHomeServiceBase : IFindMyHomeService
	{
		#region Ads

		public abstract AdsContainer SearchAds(string searchTerms, string objectTypes = null, int maxRent = 0, int maxPrice = 0, int? offset = 0, int? limit = 30, int userId = 0);

		#endregion

		#region Venues

		public abstract IEnumerable<Venue> SearchVenues(string searchTerms, string categories);

        public abstract IEnumerable<Category> RefreshCategories();

		#endregion

		#region Autocomplete

		public abstract IEnumerable<string> GetSearchTerms(string term);

		public abstract IEnumerable<string> GetVenueSearchTerms(string term);

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
