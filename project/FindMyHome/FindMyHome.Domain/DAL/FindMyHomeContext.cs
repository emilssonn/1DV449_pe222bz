using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Entities;
using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Entities.Foursquare;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FindMyHome.Domain.DAL
{
    internal class FindMyHomeContext : DbContext, IFindMyHomeContext
    {
        public IDbSet<Ad> Ads { get; set; }

        public IDbSet<AdsContainer> AdsContainers { get; set; }

        public IDbSet<UserAdsSearch> UserAdsSearches { get; set; }

        public IDbSet<Category> Categories { get; set; }


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
            modelBuilder.Entity<UserAdsSearch>().ToTable("UserAdsSearch", "dbo");
            modelBuilder.Entity<Category>().ToTable("Category", "dbo");

            //Self relation, one can have many childs, only one parent
            modelBuilder.Entity<Category>()
                .HasOptional(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId);
        }
    }
}
