using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Entities.Foursquare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Abstract
{
    public interface IFindMyHomeService : IDisposable
	{
		#region Ads

		AdsContainer SearchAds(string searchTerms, string objectTypes = null, int maxRent = 0, int maxPrice = 0, int? offset = 0, int? limit = 30, int userId = 0);

		#endregion

		#region Venues

		IEnumerable<Venue> SearchVenues(string searchTerms, string categories);

		IEnumerable<Category> RefreshCategories();

		#endregion

		#region Autocomplete

		IEnumerable<string> GetSearchTerms(string term);

		IEnumerable<string> GetVenueSearchTerms(string term);

		#endregion
	}
}
