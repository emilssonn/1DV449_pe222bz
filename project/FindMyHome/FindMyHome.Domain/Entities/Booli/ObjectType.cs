using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities.Booli
{
	public enum ObjectType
	{
		Villa, 
		Lägenhet, 
		Gård, 
		[Description("Tomt-mark")]
		Tomtmark, 
		Fritidshus, 
		Parhus,
		Radhus,
		Kedjehus
	}
}
