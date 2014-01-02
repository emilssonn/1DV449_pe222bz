using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Entities.Booli;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FindMyHome.Domain.DAL
{
    internal class FindMyHomeContext : DbContext, IFindMyHomeContext
    {
        public IDbSet<Ad> Ads { get; set; }

        public IDbSet<AdsContainer> AdsContainers { get; set; }


        static FindMyHomeContext()
        {
            Database.SetInitializer<FindMyHomeContext>(null);
        }

        public FindMyHomeContext()
            : base("name=DefaultConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Map entity types to tables.
            modelBuilder.Entity<Ad>().ToTable("Ad", "dbo");
            modelBuilder.Entity<AdsContainer>().ToTable("AdsContainer", "dbo");

        }
    }
}
