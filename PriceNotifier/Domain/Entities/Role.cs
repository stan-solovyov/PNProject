using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role : IEntityWithTypedId<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<UserRole> UserRoles { get; set; }
    }
}
