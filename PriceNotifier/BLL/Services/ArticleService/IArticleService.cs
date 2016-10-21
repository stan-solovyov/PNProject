using System.Linq;
using Domain.Entities;

namespace BLL.Services.ArticleService
{
    public interface IArticleService:IService<Article>
    {
        IQueryable<Article> GetByProductId(int productId);
        IQueryable<Article> Query();
    }
}
