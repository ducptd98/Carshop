using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
