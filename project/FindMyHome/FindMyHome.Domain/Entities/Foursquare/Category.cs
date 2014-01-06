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
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        public string Id { get; set; }

        public string EngName { get; set; }

        public string SweName { get; set; }

        public string IconPrefix { get; set; }

        public string IconSuffix { get; set; }

        public int? ParentId { get; set; }

        public Category SubCategory { get; set; }

        public virtual List<Category> SubCategories { get; set; }

        public Category()
        {
            this.SubCategories = new List<Category>();
        }

        public Category(JToken category, Category parentCategory = null)
        {
            this.SubCategories = new List<Category>();
            this.Id = (string)category["id"];
            this.EngName = (string)category["name"];
            this.IconPrefix = (string)category["icon"]["prefix"];
            this.IconSuffix = (string)category["icon"]["suffix"];
        }
    }
}
