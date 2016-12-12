using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class ProvidersProductInfo : IEntityWithTypedId<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProviderName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
