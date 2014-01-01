using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Webservices;
using FindMyHome.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain
{
    public class FindMyHomeService : FindMyHomeServiceBase
    {

        public override AdsContainer Search(string searchTerms, string objectTypes = null, int? offset = 0, int? limit = 500)
        {
            var searchTermsArray = searchTerms.Split(',').Select(s => s.Trim()).ToArray();
            searchTerms = string.Join(",", searchTermsArray);

            if (objectTypes != null)
            {
                var objectTypesArray = objectTypes.Split(',').Select(s => s.Trim()).ToArray();
                objectTypes = string.Join(",", objectTypesArray);
            }

            var booliWebservice = new BooliWebservice();

            return booliWebservice.SearchRaw(searchTerms, objectTypes, offset, limit);
        }
    }
}
