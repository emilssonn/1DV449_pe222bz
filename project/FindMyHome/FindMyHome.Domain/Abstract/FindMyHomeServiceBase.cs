using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Abstract
{
    public abstract class FindMyHomeServiceBase : IFindMyHomeService
    {
        public abstract AdsContainer Search(string searchTerms, string objectTypes = null, int? offset = 0, int? limit = 30);

        public abstract IEnumerable<string> GetSearchTerms(string term);

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
