using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace FindMyHome.Domain.Entities.Booli
{
    public partial class Ad
    {
		/// <summary>
		/// Composite primary key
		/// </summary>
        [Key, Column(Order = 2), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		/// <summary>
		/// Composite primary key
		/// </summary>
        [Key, Column(Order = 1)]
        [ForeignKey("AdsContainer")]
        [JsonIgnore]
        public int AdsContainerId { get; set; }

        [JsonIgnore]
        public virtual AdsContainer AdsContainer { get; set; }

        public int BooliId { get; set; }

        public int? ListPrice { get; set; }

        public int? Rent { get; set; }

        public DateTime Published { get; set; }

        public string ObjectType { get; set; }

        public int? Rooms { get; set; }

        public int? LivingArea { get; set; }

        public int? ConstructionYear { get; set; }

        public string BooliUrl { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public Ad() { }

		/// <summary>
		/// Get the values from the JToken
		/// All values that can be null is parsed to get the value or set to null
		/// </summary>
		/// <param name="adToken"></param>
        public Ad(JToken adToken)
        {
            int t;
            this.BooliId = (int)adToken["booliId"];
            this.ListPrice = int.TryParse((string)adToken["listPrice"], out t) ? t : default(int?);
            this.Rent = int.TryParse((string)adToken["rent"], out t) ? t : default(int?);
            this.Published = DateTime.Parse((string)adToken["published"]);
            this.ObjectType = (string)adToken["objectType"];
            this.Rooms = int.TryParse((string)adToken["rooms"], out t) ? t :default(int?);
            this.LivingArea = int.TryParse((string)adToken["livingArea"], out t) ? t :default(int?);
            this.ConstructionYear = int.TryParse((string)adToken["constructionYear"], out t) ? t :default(int?);
            this.BooliUrl = (string)adToken["url"];
            this.StreetAddress = (string)adToken["location"]["address"]["streetAddress"];
            this.City = (string)adToken["location"]["address"]["city"];
        }
    }
}
