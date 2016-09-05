using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repository;

namespace BLL.Services.UserService
{
    public class UserService : IUserService<User>
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PageResult<User>> Get(string sortDataField, string sortOrder, string filter, string filterField, int currentPage, int recordsPerPage)
        {
            var totalItems = _userRepository.Query().Count();
            var begin = (currentPage - 1) * recordsPerPage;
            var query = _userRepository.Query();

            switch (filterField)
            {
                case "Id":
                    query = query.OrderBy(x => x.Id).Where(x => x.Id.ToString().Contains(filter));
                    totalItems = query.Count();
                    break;
                case "Username":
                    query = query.OrderBy(x => x.Id).Where(x => x.Username.Contains(filter));
                    totalItems = query.Count();
                    break;
            }

            switch (sortOrder)
            {
                case "asc":
                        if (sortDataField == "Id")
                        {
                            query =  query.OrderBy(x => x.Id);
                        }

                        if (sortDataField == "Username")
                        {
                            query = query.OrderBy(x => x.Username);
                        }

                    break;

                case "desc":
                        if (sortDataField == "Id")
                        {
                            query = query.OrderByDescending(x => x.Id);
                        }

                        if (sortDataField == "Username")
                        {
                            query = query.OrderByDescending(x => x.Username);
                        }

                    break;
            }

            if (filterField == "Id" || filterField == "Username")
            {
                return new PageResult<User> {Data = await query.Skip(begin).Take(recordsPerPage).ToListAsync(), TotalItems = totalItems};
            }

            if (sortOrder == "asc" || sortOrder == "desc")
            {
                return new PageResult<User> { Data = await query.Skip(begin).Take(recordsPerPage).ToListAsync(), TotalItems = totalItems };
            }
            return new PageResult<User>
            {
                Data = await query.OrderBy(x => x.Id).Skip(begin).Take(recordsPerPage).ToListAsync(),
                TotalItems = totalItems
            };
        }

        public async Task<User> Create(User user)
        {
            await _userRepository.Create(user);
            return user;
        }

        public async Task Update(User user)
        {
            await _userRepository.Update(user);
        }

        public async Task<User> GetById(int userId)
        {
            User user = await _userRepository.Get(userId);
            return user;
        }

        public async Task Delete(User user)
        {
            await _userRepository.Delete(user);
        }
    }
}
