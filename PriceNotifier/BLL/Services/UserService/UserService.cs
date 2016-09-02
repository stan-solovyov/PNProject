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

        public async Task<IEnumerable<User>> Get(string sortDataField, string sortOrder, string filter, string filterField)
        {
            var query = _userRepository.Query();

            switch (filterField)
            {
                case "Id":
                    query = query.Where(x => x.Id.ToString().Contains(filter));
                    break;
                case "Username":
                    query = query.Where(x => x.Username.Contains(filter));
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
            return await query.ToListAsync();
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
