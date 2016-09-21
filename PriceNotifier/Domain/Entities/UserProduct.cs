using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class UserProduct
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
        public bool Checked { get; set; }
    }
}
