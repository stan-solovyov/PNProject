using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
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
        public PageResult<PriceHistoryDto> GetPriceHistory(int id,string name, ODataQueryOptions<PriceHistory> options)
        {
            var allPriceHistories = _priceHistoryService.GetByProductIdAndProvider(id,name);
            IQueryable priceHistories = options.ApplyTo(allPriceHistories);
            var results = priceHistories.ProjectTo<PriceHistoryDto>();

            return new PageResult<PriceHistoryDto>(
                results,
                Request.ODataProperties().NextLink,
                Request.ODataProperties().TotalCount);
        }
    }
}