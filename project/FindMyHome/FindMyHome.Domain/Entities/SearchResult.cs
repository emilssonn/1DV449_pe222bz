using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Entities.Foursquare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities
{
	/// <summary>
	/// The combined result from a search
	/// </summary>
	public class SearchResult
	{
		public AdsContainer AdsContainer { get; set; }

		public List<Venue> Venues { get; set; }

		public SearchResult(AdsContainer adsContainer, List<Venue> venues = null)
		{
			//ALways have a list of venues, even if its empty
			this.AdsContainer = adsContainer;
			if (venues != null)
				this.Venues = venues;
			else
				this.Venues = new List<Venue>();
		}
	}
}
