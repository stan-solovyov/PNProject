using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Services.UserService
{
    public interface IUserService:IService<User>
    {
        Task<PageResult<UserFromDbWithCount>> Get(string sortDataField, string sortOrder, string filter, string filterField, int currentPage, int recordsPerPage);
    }
}
