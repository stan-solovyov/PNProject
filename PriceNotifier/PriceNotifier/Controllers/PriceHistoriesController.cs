using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using BLL.Services.PriceHistoryService;
using Domain.Entities;
using PriceNotifier.DTO;

namespace PriceNotifier.Controllers
{
    public class PriceHistoriesController : ApiController
    {
        private readonly IPriceHistoryService _priceHistoryService;

        public PriceHistoriesController(IPriceHistoryService priceHistoryService)
        {
            _priceHistoryService = priceHistoryService;
        }

        // GET: api/PriceHistories/5
        public async Task<IEnumerable<PriceHistoryDto>> GetPriceHistory(int id)
        {
            var priceHistories = await _priceHistoryService.GetByProductId(id);
            if (priceHistories == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var priceHistoriesDto = Mapper.Map<IEnumerable<PriceHistoryDto>>(priceHistories);

            return priceHistoriesDto;
        }

        // POST: api/PriceHistories
        [ResponseType(typeof(PriceHistoryDto))]
        public async Task<PriceHistoryDto> PostPriceHistory(PriceHistoryDto priceHistoryDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var priceHistory = Mapper.Map<PriceHistoryDto, PriceHistory>(priceHistoryDto);
            var priceHistoryUpdated = await _priceHistoryService.Create(priceHistory);
            priceHistoryDto = Mapper.Map(priceHistoryUpdated, priceHistoryDto);

            return priceHistoryDto;
        }
    }
}