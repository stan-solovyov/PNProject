using System.Data.Entity;
using System.Linq;
using Domain.EF;
using Domain.Entities;
using MoreLinq;
using PriceNotifier.Infrostructure;

namespace ElasticIndexBuilder
{
    public class ElasticIndex : IElasticIndex
    {
        private readonly IElasticService<Product> _elasticProductService;
        private readonly IElasticService<User> _elasticUserService;
        private readonly UserContext _db;

        public ElasticIndex(IElasticService<Product> elasticProductService, IElasticService<User> elasticUserService, UserContext db)
        {
            _elasticProductService = elasticProductService;
            _elasticUserService = elasticUserService;
            _db = db;
        }

        public void BuildIndexes()
        {
            //initializing productindex
            var users = _db.Users.AsNoTracking().Include(a=>a.UserProducts).OrderBy(t => t.UserId).Batch(5000);
            foreach (var batch in users)
            {
                _elasticUserService.AddToIndexMany(batch);
            }

            //initializing productindex
            var products = _db.Products.AsNoTracking().Include(d=>d.Articles).Include(d => d.ProvidersProductInfos).Include(d => d.UserProducts).OrderBy(t => t.ProductId).Batch(5000);
            foreach (var batch in products)
            {
                _elasticProductService.AddToIndexMany(batch);
            }
        }

        public void DeleteIndexes()
        {
            _elasticProductService.DeleteIndex();
            _elasticUserService.DeleteIndex();
        }
    }
}
