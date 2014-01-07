using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Entities.Foursquare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities
{
	public class SearchResult
	{
		public AdsContainer AdsContainer { get; set; }

		public List<Venue> Venues { get; set; }

		public SearchResult(AdsContainer adsContainer, List<Venue> venues = null)
		{
			this.AdsContainer = adsContainer;
			if (venues != null)
				this.Venues = venues;
			else
				this.Venues = new List<Venue>();
		}
	}
}
