using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }

        [MaxLength(450)]
        [Required]
        public string ExternalProductId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public virtual List<PriceHistory> PriceHistories { get; set; }
        public virtual List<UserProduct> UserProducts { get; set; }
    }
}
