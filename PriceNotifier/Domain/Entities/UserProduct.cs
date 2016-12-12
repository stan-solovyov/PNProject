using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class UserProduct
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [JsonIgnore]
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public bool Checked { get; set; }
    }
}
