namespace PriceNotifier.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string ExternalProductId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool Checked { get; set; }
    }
}