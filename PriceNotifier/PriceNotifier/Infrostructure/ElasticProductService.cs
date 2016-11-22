using Domain.Entities;

namespace PriceNotifier.Infrostructure
{
    public class ElasticProductService : ElasticService<Product>
    {
        public ElasticProductService(string elastiSearchServerUrl) : base(elastiSearchServerUrl, "productindex", d => d.Name)
        {

        }
    }
}