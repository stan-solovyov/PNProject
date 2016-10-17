using System;

namespace PriceNotifier.DTO
{
    public class PriceHistoryDto
    {
        public int PriceHistoryId { get; set; }
        public DateTime Date { get; set; }
        public double? OldPrice { get; set; }
        public double? NewPrice { get; set; }
        public int ProviderId { get; set; }
    }
}