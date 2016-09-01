using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repository;

namespace BLL.Services.UserService
{
    public class UserService: IUserService<User>
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Get(string sortDataField, string sortOrder)
        {
            if (sortOrder == "asc")
            {
                switch (sortDataField)
                {
                    case "Id":
                        return await _userRepository.Query().OrderBy(x => x.Id).ToListAsync();
                    case "Username":
                        return await _userRepository.Query().OrderBy(x => x.Username).ToListAsync();
                }
            }
            else
            {
                switch (sortDataField)
                {
                    case "Id":
                        return await _userRepository.Query().OrderByDescending(x => x.Id).ToListAsync();
                    case "Username":
                        return await _userRepository.Query().OrderByDescending(x => x.Username).ToListAsync();
                }
            }
            return await _userRepository.Query().ToListAsync();
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
