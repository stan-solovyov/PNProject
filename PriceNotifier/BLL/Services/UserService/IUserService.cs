using System.Linq;
using BLL.Models;
using Domain.Entities;

namespace BLL.Services.UserService
{
    public interface IUserService : IService<User>
    {
        IQueryable<UserFromDbWithCount> Get();
    }
}
