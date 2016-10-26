using Domain.Entities;

namespace PriceNotifier.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string ExternalProductId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool Checked { get; set; }
        public Article Article { get; set; }
    }
}