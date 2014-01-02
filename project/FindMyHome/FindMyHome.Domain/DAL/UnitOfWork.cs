using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.DAL
{
    internal class UnitOfWork : IUnitOfWork
    {
        private FindMyHomeContext _context = new FindMyHomeContext();

        private IRepository<Ad> _adRepository;

        private IRepository<AdsContainer> _adsContainerRepository;

        public IRepository<Ad> AdRepository
        {
            get
            {
                return this._adRepository ?? (this._adRepository = new Repository<Ad>(this._context));
            }
        }

        public IRepository<AdsContainer> AdsContainerRepository
        {
            get
            {
                return this._adsContainerRepository ?? (this._adsContainerRepository = new Repository<AdsContainer>(this._context));
            }
        }

        public void Save()
        {
            this._context.SaveChanges();
        }

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
