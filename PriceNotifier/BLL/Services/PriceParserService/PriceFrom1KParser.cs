using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Entities;
using HtmlAgilityPack;

namespace BLL.Services.PriceParserService
{
    public class PriceFrom1KParser : IProviderProductInfoParser
    {
        public async Task<ProvidersProductInfo> GetProvidersProductInfo(string productName)
        {
            string html, uri = "", address = "http://1k.by/products/search?searchFor=products&s_keywords=";
            double? minPrice = null;
            double? maxPrice = null;
            string imageUrl = null;
            using (var client = new HttpClient())
            {
                html = await client.GetStringAsync(address + productName);
            }

            if (html != null)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var node = doc.QuerySelectorAll(".pr-line_about a").FirstOrDefault();
                if (node?.Attributes["href"] != null)
                {
                    uri = node.Attributes["href"].Value;
                }

                if (!string.IsNullOrEmpty(uri))
                {
                    using (var client = new HttpClient())
                    {
                        html = await client.GetStringAsync(uri);
                    }

                    doc.LoadHtml(html);
                    var allNodes = doc.QuerySelectorAll(".price");
                    var nodes = allNodes.QuerySelectorAll("span [itemprop=lowPrice]").FirstOrDefault();

                    if (!string.IsNullOrEmpty(nodes?.InnerText))
                    {
                        var price = nodes.InnerText;
                        minPrice = double.Parse(price);
                    }

                    nodes = allNodes.QuerySelectorAll("span [itemprop=highPrice]").FirstOrDefault();
                    if (!string.IsNullOrEmpty(nodes?.InnerText))
                    {
                        var price = nodes.InnerText;
                        maxPrice = double.Parse(price);
                    }

                    nodes = doc.QuerySelectorAll(".product_img [itemprop=image]").FirstOrDefault();
                    if (!string.IsNullOrEmpty(nodes?.GetAttributeValue("src", "")))
                    {
                        imageUrl = nodes.GetAttributeValue("src", "");
                    }

                    ProvidersProductInfo p = new ProvidersProductInfo
                    {
                        ProviderName = "1K",
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
