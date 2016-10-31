using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.EF;
using Domain.Entities;

namespace Domain.Repository
{
    public class ArticleRepository:IRepository<Article>
    {
        private readonly UserContext _context;

        public ArticleRepository(UserContext context)
        {
            _context = context;
        }

        public IQueryable<Article> Query()
        {
            return _context.Articles;
        }

        public async Task<Article> Get(int id)
        {
            return await _context.Articles.Include(c=>c.Product).SingleAsync(c=>c.ArticleId==id);
        }

        public Task Update(Article entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task<Article> Create(Article entity)
        {
            _context.Articles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task Delete(Article entity)
        {
            _context.Articles.Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}
