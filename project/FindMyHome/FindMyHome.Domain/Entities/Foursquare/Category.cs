using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Entities.Foursquare
{
    public class Category : IEquatable<Category>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        public string Id { get; set; }

        public string EngName { get; set; }

		/// <summary>
		/// Swedish translation of the name returned from Foursquare
		/// </summary>
        public string SweName { get; set; }

		/// <summary>
		/// Name to be displayed to the application
		/// </summary>
		public string DisplayName { get; set; }

        public string IconPrefix { get; set; }

        public string IconSuffix { get; set; }

        public int? ParentId { get; set; }

        public Category ParentCategory { get; set; }

        public virtual List<Category> SubCategories { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime LastUpdated { get; set; }

        public Category()
        {
            this.SubCategories = new List<Category>();
        }

        public Category(JToken category, Category parentCategory = null)
        {
            this.SubCategories = new List<Category>();
            this.Id = (string)category["id"];
            this.EngName = (string)category["name"];
			this.DisplayName = (string)category["name"];
            this.IconPrefix = (string)category["icon"]["prefix"];
            this.IconSuffix = (string)category["icon"]["suffix"];
        }

		/// <summary>
		/// Compare 2 categorys by the specific values
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
        public bool Equals(Category other)
        {
			//Same instance of the object
			if (Object.ReferenceEquals(this, other)) 
				return true;

			return
				this.Id == other.Id &&
				this.EngName == other.EngName &&
				this.IconPrefix == other.IconPrefix &&
				this.IconSuffix == other.IconSuffix &&
				this.ParentCategory == other.ParentCategory;
        }
    }
}
