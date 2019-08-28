using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.Models
{
    public class ProductLike
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }

        public DateTime AddedDate { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUsers ApplicationUsers { get; set; }

    }
    public class ProductLikeMap
    {
        public ProductLikeMap(EntityTypeBuilder<ProductLike> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => new { x.ProductId, x.UserId });
            //entityTypeBuilder.Property(x => x.AddedDate).HasDefaultValue(DateTime.Now);
        }
    }
}
