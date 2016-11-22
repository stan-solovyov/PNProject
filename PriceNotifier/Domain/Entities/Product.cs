using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(450)]
        [Required]
        public string ExternalProductId { get; set; }
        public virtual List<UserProduct> UserProducts { get; set; }
        public virtual List<ProvidersProductInfo> ProvidersProductInfos { get; set; }
        public virtual List<Article> Articles { get; set; }

        public Product()
        {
            ProvidersProductInfos = new List<ProvidersProductInfo>();
            Articles = new List<Article>();
        }
    }
}
