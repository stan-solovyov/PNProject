using User = Domain.Entities.User;

namespace PriceNotifier.Infrostructure
{
    public class ElasticUserService : ElasticService<User>
    {
        public ElasticUserService(string elastiSearchServerUrl) : base(elastiSearchServerUrl, "userindex", d => d.Username)
        {

        }
    }
}