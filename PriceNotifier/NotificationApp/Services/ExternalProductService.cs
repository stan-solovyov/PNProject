using System.Net.Http;
using System.Threading.Tasks;
using NotificationApp.Interfaces;
using NotificationApp.Parsers;

namespace NotificationApp.Services
{
    public class ExternalProductService: IExternalProductService
    {

        private readonly IParser _parseService;

        public ExternalProductService(IParser parseService)
        {
            _parseService = parseService;
        }

        public async Task<string> GetExternalPrductPage(string address)
        {
            string html;
            using (var client = new HttpClient())
            {
                html = await client.GetStringAsync(address);
            }
            return html;
        }

        public double ParsePrice(string html)
        {
            return _parseService.Parse(html);
        }

    }
}
