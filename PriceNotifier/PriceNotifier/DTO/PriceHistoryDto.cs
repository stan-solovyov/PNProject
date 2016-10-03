using System;
using Domain.Entities;

namespace PriceNotifier.DTO
{
    public class PriceHistoryDto
    {
        public int PriceHistoryId { get; set; }
        public DateTime Date { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public int ProductId { get; set; }
    }
}