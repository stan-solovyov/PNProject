using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Entities;
using HtmlAgilityPack;

namespace BLL.Services.PriceParserService
{
    public class PriceFromMigomParser : IProviderProductInfoParser
    {
        public async Task<ProvidersProductInfo> GetProvidersProductInfo(string productName)
        {
            string html = null,
                uri = "",
                address = "http://www.migom.by/search/?search_user=&search_type=products&search_str=";
            double minPrice = 0;
            double maxPrice = 0;
            string imageUrl = null;
            using (var client = new HttpClient())
            {
                html = await client.GetStringAsync(address + productName);
            }

            if (html != null)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var nodes = doc.QuerySelectorAll("h2 a").FirstOrDefault();
                if (nodes?.Attributes["href"] != null)
                {
                    uri = "http://www.migom.by" + nodes.Attributes["href"].Value;
                }

                if (!string.IsNullOrEmpty(uri))
                {
                    using (var client = new HttpClient())
                    {
                        html = await client.GetStringAsync(uri);
                    }

                    doc.LoadHtml(html);
                    var allNodes = doc.QuerySelectorAll(".price");
                    nodes = allNodes.QuerySelectorAll("span [itemprop=lowPrice]").First();
                    if (!string.IsNullOrEmpty(nodes?.InnerText))
                    {
                        var price = nodes.InnerText;
                        price = price.Replace('.', ',');
                        minPrice = double.Parse(price);
                    }

                    nodes = allNodes.QuerySelectorAll("span [itemprop=highPrice]").First();
                    if (!string.IsNullOrEmpty(nodes?.InnerText))
                    {
                        var price = nodes.InnerText;
                        price = price.Replace('.', ',');
                        maxPrice = double.Parse(price);
                    }

                    nodes = doc.QuerySelectorAll(".b-item-card__top-img-i [itemprop=image]").First();
                    if (!string.IsNullOrEmpty(nodes?.GetAttributeValue("src", "")))
                    {
                        imageUrl = nodes.GetAttributeValue("src", "");
                    }

                    ProvidersProductInfo p = new ProvidersProductInfo
                    {
                        ProviderName = "Migom",
                        MaxPrice = maxPrice,
                        MinPrice = minPrice,
                        ImageUrl = imageUrl,
                        Url = uri
                    };

                    return p;
                }
            }

            return null;
        }
    }
}
