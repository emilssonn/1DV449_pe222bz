using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
	public class IndexViewModel
	{
		public List<string> LastSearches { get; set; }

		public IndexViewModel()
		{
			this.LastSearches = new List<string>();
		}
	}
}