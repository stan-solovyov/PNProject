using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task Update(PriceHistory entity)
        {
            await _priceHistoryRepository.Update(entity);
        }

        public async Task<PriceHistory> GetById(int priceHistoryId)
        {
            return await _priceHistoryRepository.Get(priceHistoryId);
        }

        public async Task Delete(PriceHistory entity)
        {
            await _priceHistoryRepository.Delete(entity);
        }

        public async Task<IEnumerable<PriceHistory>> GetByProductId(int productId)
        {
            return await _priceHistoryRepository.Query().Where(c => c.Product.ProductId == productId).ToListAsync();
        }
    }
}
