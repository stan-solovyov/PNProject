using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        [MaxLength(450)]
        public string ExternalProductId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public virtual List<PriceHistory> PriceHistories { get; set; }
        public virtual List<UserProduct> UserProducts { get; set; }
    }
}
