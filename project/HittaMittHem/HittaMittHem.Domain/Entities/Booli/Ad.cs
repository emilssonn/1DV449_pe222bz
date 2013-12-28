using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HittaMittHem.Domain.Entities.Booli
{
    public partial class Ad
    {
        public int BooliId { get; set; }

        public int ListPrice { get; set; }

        public DateTime Published { get; set; }

        public string ObjectType { get; set; }

        public int Rooms { get; set; }

        public int LivingArea { get; set; }

        public int ConstructionYear { get; set; }

        public string BooliUrl { get; set; }

        public Ad() { }

        public Ad(JToken adToken)
        {
            this.BooliId = (int)adToken["booliId"];
            this.ListPrice = (int)adToken["listPrice"];
            this.Published = DateTime.Parse((string)adToken["published"]);
            this.ObjectType = (string)adToken["objectType"];
            this.Rooms = (int)adToken["rooms"];
            this.LivingArea = (int)adToken["livingArea"];
            this.ConstructionYear = adToken["constructionYear"] != null ? (int)adToken["constructionYear"] : 0;
            this.BooliUrl = (string)adToken["url"];
        }
    }
}
