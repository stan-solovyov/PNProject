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
            var users = _db.Users.AsNoTracking().OrderBy(t => t.UserId).Batch(500);
            foreach (var batch in users)
            {
                foreach (var u in batch)
                {
                    _elasticUserService.AddToIndex(u, u.UserId);
                }
            }

            //initializing productindex
            var products = _db.Products.AsNoTracking().OrderBy(t => t.ProductId).Batch(500);
            foreach (var batch in products)
            {
                foreach (var product in batch)
                {
                    _elasticProductService.AddToIndex(product, product.ProductId);
                }
            }
        }

        public void DeleteIndexes()
        {
            _elasticProductService.DeleteIndex();
            _elasticUserService.DeleteIndex();
        }
    }
}
