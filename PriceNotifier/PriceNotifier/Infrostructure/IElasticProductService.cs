using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceNotifier.Infrostructure
{
    public interface IElasticProductService<in Product>
    {
        void AddToIndexManyProducts(IEnumerable<Product> docs);
        Task DeleteIndex();
    }
}
