using FindMyHome.Domain.Entities;
using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Entities.Foursquare;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Abstract
{
    public interface IFindMyHomeContext
    {
        IDbSet<Ad> Ads { get; set; }
        IDbSet<AdsContainer> AdsContainers { get; set; }
        IDbSet<UserAdsSearch> UserAdsSearches { get; set; }
        IDbSet<Category> Categories { get; set; }
    }
}
