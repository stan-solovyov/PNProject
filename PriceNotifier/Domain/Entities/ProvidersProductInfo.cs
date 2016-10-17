using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ProvidersProductInfo
    {
        [Key]
        public int ProviderId { get; set; }
        [Required]
        public string ProviderName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
