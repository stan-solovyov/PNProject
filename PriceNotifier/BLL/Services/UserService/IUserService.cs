using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.UserService
{
    public interface IUserService<TUser> 
    {
        Task<IEnumerable<TUser>> Get(string sortDataField, string sortOrder,string filter,string filterField, int currentPage, int recordsPerPage);
        Task<TUser> Create(TUser user);
        Task Update(TUser user);
        Task<TUser> GetById(int id);
        Task Delete(TUser user);
    }
}
