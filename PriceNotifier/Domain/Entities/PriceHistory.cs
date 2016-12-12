using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class PriceHistory : IEntityWithTypedId<int>
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public double? OldPrice { get; set; }
        public double? NewPrice { get; set; }
        [Required]
        public int ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public ProvidersProductInfo ProvidersProductInfo { get; set; }
    }
}
