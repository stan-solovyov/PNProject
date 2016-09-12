using System.Linq;
using Domain.Entities;

namespace BLL.Services.UserService
{
    public interface IUserService:IService<User>
    {
        IQueryable<UserFromDbWithCount> Get();
    }
}
