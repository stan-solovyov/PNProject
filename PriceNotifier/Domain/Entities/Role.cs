using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public virtual List<UserRole> UserRoles { get; set; }
    }
}
