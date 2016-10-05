using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public PageResult<PriceHistoryDto> GetPriceHistory(int id, ODataQueryOptions<PriceHistory> options)
        {
            var allPriceHistories = _priceHistoryService.GetByProductId(id);
            IQueryable priceHistories = options.ApplyTo(allPriceHistories);
            var results = priceHistories.ProjectTo<PriceHistoryDto>();

            return new PageResult<PriceHistoryDto>(
                results,
                Request.ODataProperties().NextLink,
                Request.ODataProperties().TotalCount);
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