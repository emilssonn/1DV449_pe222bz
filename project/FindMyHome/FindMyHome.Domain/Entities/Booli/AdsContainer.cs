using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities.Booli
{
    public class AdsContainer
    {
        private List<Ad> _ads = new List<Ad>();

        public List<Ad> Ads 
        {
            get
            {
                return this._ads;
            }
            set
            {
                this._ads = value;
            }
        }

        public int TotalCount { get; set; }

        public int CurrentCount { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public string Uri { get; set; }

        public DateTime NextUpdate { get; set; }
    }
}
