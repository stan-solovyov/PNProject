using System.Net.Http;
using System.Threading.Tasks;
using NotificationApp.Interfaces;
using NotificationApp.Parsers;

namespace NotificationApp.Services
{
    public class ExternalProductService: IExternalProductService
    {

        private readonly IPriceParser _parseService;

        public ExternalProductService(IPriceParser parseService)
        {
            _parseService = parseService;
        }

        public async Task<double?> ParsePrice(string address)
        {
            string html;
            using (var client = new HttpClient())
            {
                try
                {
                    html = await client.GetStringAsync(address);
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException(ex.Message + ex.InnerException);
                }
            }

            var price = _parseService.Parse(html);
            return price;
        }

    }
}
