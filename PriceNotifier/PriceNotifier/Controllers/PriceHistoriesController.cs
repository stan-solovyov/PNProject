using System.Collections.Generic;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using AutoMapper.QueryableExtensions;
using BLL.Services.PriceHistoryService;
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

        // GET: api/PriceHistories/5/Migom
        public PageResult<PriceHistoryDto> GetPriceHistory(int id, string name, ODataQueryOptions<PriceHistoryDto> options)
        {
            var allPriceHistories = _priceHistoryService.GetByProductIdAndProvider(id, name);
            var priceHistories = allPriceHistories.ProjectTo<PriceHistoryDto>();
            var results = options.ApplyTo(priceHistories);

            return new PageResult<PriceHistoryDto>(
                results as IEnumerable<PriceHistoryDto>,
                Request.ODataProperties().NextLink,
                Request.ODataProperties().TotalCount);
        }
    }
}