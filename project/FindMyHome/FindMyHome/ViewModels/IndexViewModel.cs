using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyHome.ViewModels
{
	public class IndexViewModel
	{
		//Last searches made by the user
		public List<string> LastSearches { get; set; }

		/// <summary>
		/// Return all add object types
		/// Get the description or the name of the enum
		/// </summary>
		public List<string> ObjectTypes
		{
			get
			{
				var objectTypes = new List<string>();

				foreach (var item in Enum.GetValues(typeof(ObjectType)))
				{
					objectTypes.Add(EnumHelper.GetDescriptionOrValue((ObjectType)item));
				}
				return objectTypes;
			}
		}

		public IndexViewModel()
		{
			this.LastSearches = new List<string>();
		}
	}
}