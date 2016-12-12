using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [JsonIgnore]
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
