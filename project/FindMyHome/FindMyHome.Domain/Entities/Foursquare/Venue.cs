using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities.Foursquare
{
	public class Venue
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<String> Categories { get; set; }

		public Venue(JToken token)
		{
			this.Id = (string)token["id"];
			this.Name = (string)token["name"];
			this.Categories = new List<string>();
			this.Categories.AddRange(token["categories"].Select(c => (string)c["name"]).ToList());
		}
	}
}
