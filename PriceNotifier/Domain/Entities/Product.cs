using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }

        [MaxLength(450)]
        [Index("IX_ExtIdAndUserId", Order = 2)]
        public string ExternalProductId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool Checked { get; set; }

        [Index("IX_ExtIdAndUserId", Order = 1)]
        public int UserId { get; set; }
    }
}
