using System;

namespace Domain.Entities
{
    public class PriceHistory
    {
        public int PriceHistoryId { get; set; }
        public DateTime  Date { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public Product Product { get; set; }
    }
}
