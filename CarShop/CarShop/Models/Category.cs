using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public Category CategoryParent { get; set; }
        public ICollection<Category> CategoriesList { get; set; }


    }
}
