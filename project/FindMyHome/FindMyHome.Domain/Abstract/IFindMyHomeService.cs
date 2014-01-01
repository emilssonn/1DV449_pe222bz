using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Abstract
{
    public interface IFindMyHomeService : IDisposable
    {
        AdsContainer Search(string searchTerms, string objectTypes = null, int? offset = 0, int? limit = 500);
    }
}
