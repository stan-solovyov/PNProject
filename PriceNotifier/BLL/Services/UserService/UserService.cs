using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<IEnumerable<User>> Get()
        {
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
            return user; ;
        }

        public async Task Delete(User user)
        {
            await _userRepository.Delete(user);
        }
    }
}
