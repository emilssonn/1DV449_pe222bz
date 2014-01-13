using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities.Booli
{
    public partial class AdsContainer
    {
        [JsonIgnore]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual List<Ad> Ads { get; set; }

        public int TotalCount { get; set; }

        public int CurrentCount { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public string SearchTerms { get; set; }

        public string ObjectTypes { get; set; }

        public int MaxRent { get; set; }

        public int MaxPrice { get; set; }

        public DateTime NextUpdate { get; set; }

        public DateTime LastUpdate { get; set; }

        public AdsContainer()
        {
            this.Ads = new List<Ad>();
        }
    }
}
