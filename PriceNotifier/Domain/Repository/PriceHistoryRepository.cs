using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.EF;
using Domain.Entities;

namespace Domain.Repository
{
    public class PriceHistoryRepository : IRepository<PriceHistory>
    {
        private readonly UserContext _context;

        public PriceHistoryRepository(UserContext context)
        {
            _context = context;
        }

        public IQueryable<PriceHistory> Query()
        {
            return _context.PriceHistories;
        }

        public async Task<PriceHistory> Get(int id)
        {
            return await _context.PriceHistories.FindAsync(id);
        }

        public Task Update(PriceHistory entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<PriceHistory> Create(PriceHistory entity)
        {
            _context.PriceHistories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task Delete(PriceHistory entity)
        {
            _context.PriceHistories.Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}
