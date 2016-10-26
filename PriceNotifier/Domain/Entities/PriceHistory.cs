using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PriceHistory
    {
        public int PriceHistoryId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public double? OldPrice { get; set; }
        public double? NewPrice { get; set; }
        [Required]
        public int ProviderId { get; set; }
        public ProvidersProductInfo ProvidersProductInfo { get; set; }
    }
}
