using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.EF;
using Domain.Entities;

namespace Domain.Repository
{
    public class UserRepository:IRepository<User>
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public IQueryable<User> Query()
        {
            return _context.Users;
        }

        public async Task<User> Get(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
