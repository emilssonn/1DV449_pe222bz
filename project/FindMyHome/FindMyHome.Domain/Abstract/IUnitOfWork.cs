using FindMyHome.Domain.Entities;
using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FindMyHome.Domain.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Ad> AdRepository { get; }
        IRepository<AdsContainer> AdsContainerRepository { get; }
        IRepository<UserAdsSearch> UserAdsSearchRepository { get; }
        void Save();
    }
}
