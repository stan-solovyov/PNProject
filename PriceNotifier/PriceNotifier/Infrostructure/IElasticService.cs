using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace PriceNotifier.Infrostructure
{
    public interface IElasticService<T> where T : class
    {
        IQueryable<T> SearchProducts(string query);
        Task DeleteIndex();
        void DeleteFromIndex(int id);
        IUpdateResponse<T> UpdateDoc(int id, T doc);
        void AddToIndex(T doc, int id);
        void AddToIndexMany(IEnumerable<T> docs);
    }
}
