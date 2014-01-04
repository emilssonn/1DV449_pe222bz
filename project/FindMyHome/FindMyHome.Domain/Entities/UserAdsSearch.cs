using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities
{
    public class UserAdsSearch
    {
        [Key,  Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime SearchTime { get; set; }

        [ForeignKey("AdsContainer")]
        public int AdsContainerId { get; set; }

        public virtual AdsContainer AdsContainer { get; set; }

        public UserAdsSearch() { }

        public UserAdsSearch(int userId, int adsContainerId)
        {
            this.UserId = userId;
            this.AdsContainerId = adsContainerId;
        }

        
    }
}
