using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repository;

namespace BLL.Services.PriceHistoryService
{
    public class PriceHistoryService : IPriceHistoryService
    {
        private readonly IRepository<PriceHistory> _priceHistoryRepository;

        public PriceHistoryService(IRepository<PriceHistory> priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task<PriceHistory> Create(PriceHistory entity)
        {
            await _priceHistoryRepository.Create(entity);
            return entity;
        }

        public Task Update(PriceHistory entity)
        {
            return _priceHistoryRepository.Update(entity);
        }

        public async Task<PriceHistory> GetById(int priceHistoryId)
        {
            return await _priceHistoryRepository.Get(priceHistoryId);
        }

        public Task Delete(PriceHistory entity)
        {
            return _priceHistoryRepository.Delete(entity);
        }

        public IQueryable<PriceHistory> GetByProductIdAndProvider(int productId, string provider)
        {
            return _priceHistoryRepository.Query().Where(c => c.ProvidersProductInfo.ProductId == productId && c.ProvidersProductInfo.ProviderName == provider);
        }
    }
}
