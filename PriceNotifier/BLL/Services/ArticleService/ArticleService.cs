using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repository;

namespace BLL.Services.ArticleService
{
    public class ArticleService:IArticleService
    {
        private readonly IRepository<Article> _articleRepository;

        public ArticleService(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Article> Create(Article entity)
        {
            await _articleRepository.Create(entity);
            return entity;
        }

        public Task Update(Article entity)
        {
            return _articleRepository.Update(entity);
        }

        public async Task<Article> GetById(int id)
        {
            return await _articleRepository.Get(id);
        }

        public Task Delete(Article entity)
        {
            return _articleRepository.Delete(entity);
        }

        public IQueryable<Article> GetByProductId(int productId)
        {
            return _articleRepository.Query().Where(c => c.ProductId == productId);
        }

        public IQueryable<Article> Query()
        {
            return _articleRepository.Query();
        }
    }
}
