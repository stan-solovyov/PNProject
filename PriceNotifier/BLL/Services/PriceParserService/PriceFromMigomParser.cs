using System.Net.Http;
using System.Threading.Tasks;
using Domain.Entities;
using HtmlAgilityPack;

namespace BLL.Services.PriceParserService
{
    public class PriceFromMigomParser : IMigomPriceParser
    {
        public async Task<string> GetProdLinks(string productName)
        {
            string html = null, uri = "", address = "http://www.migom.by/search/?search_user=&search_type=products&search_str=";
            using (var client = new HttpClient())
            {
                html = await client.GetStringAsync(address + productName);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[5]/div[1]/div[3]/div[1]/div/div[2]/div[1]/div/div[2]/h2/a");
            if (nodes.Count == 1)
            {
                if (nodes[0].Attributes["href"] != null)
                {
                    return uri = "http://www.migom.by" + nodes[0].Attributes["href"].Value;
                }
                return uri;
            }

            return uri;
        }

        public async Task<string> GetExternalPrductPage(string address)
        {
            string html;
            if (!string.IsNullOrEmpty(address))
            {
                using (var client = new HttpClient())
                {
                    html = await client.GetStringAsync(address);
                }
                return html;
            }
            return "";
        }

        public ProvidersProductInfo ParsePrice(string html)
        {
            double minPrice = 0, maxPrice = 0;
            string imageUrl = null;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodes = doc.DocumentNode.SelectNodes("//*[@id='product']/div[1]/div/div/div[2]/div/div[1]/div[1]/a[1]/span");
            if (nodes.Count == 1)
            {
                if (!string.IsNullOrEmpty(nodes[0].InnerText))
                {
                    var price = nodes[0].InnerText;
                    price = price.Replace('.', ',');
                    minPrice = double.Parse(price);
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//*[@id='product']/div[1]/div/div/div[2]/div/div[1]/div[1]/a[2]/span");
            if (nodes.Count == 1)
            {
                if (!string.IsNullOrEmpty(nodes[0].InnerText))
                {
                    var price = nodes[0].InnerText;
                    price = price.Replace('.', ',');
                    maxPrice = double.Parse(price);
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//*[@id='product']/div[1]/div/div/div[1]/figure/div/a/img");
            if (nodes.Count == 1)
            {
                    if (!string.IsNullOrEmpty(nodes[0].GetAttributeValue("src", "")))
                    {
                        imageUrl = nodes[0].GetAttributeValue("src", "");
                    }
            }

            ProvidersProductInfo p = new ProvidersProductInfo { ProviderName = "Migom", MaxPrice = maxPrice, MinPrice = minPrice, ImageUrl = imageUrl };

            return p;
        }
    }
}
