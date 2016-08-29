using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.UserService
{
    public interface IUserService<User> 
    {
        Task<IEnumerable<User>> Get();
        Task<User> Create(User user);
        Task Update(User user);
        Task<User> GetById(int id);
        Task Delete(User user);
    }
}
