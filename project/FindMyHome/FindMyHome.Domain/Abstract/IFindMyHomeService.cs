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
        AdsContainer Search(string searchTerms, string objectTypes = null, int maxRent = 0, int maxPrice = 0, int? offset = 0, int? limit = 30, int userId = 0);

        IEnumerable<string> GetSearchTerms(string term);

        IEnumerable<Category> RefreshCategories();
    }
}
