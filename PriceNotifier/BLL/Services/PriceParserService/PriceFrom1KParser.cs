using System.Net.Http;
using System.Threading.Tasks;
using Domain.Entities;
using HtmlAgilityPack;

namespace BLL.Services.PriceParserService
{
    public class PriceFrom1KParser:IOneKPriceParser
    {
        public async Task<string> GetProdLinks(string productName)
        {
            string html, uri = "", address = "http://1k.by/products/search?searchFor=products&s_keywords=";
            using (var client = new HttpClient())
            {
                html = await client.GetStringAsync(address + productName);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (doc.DocumentNode.SelectNodes("//*[@id='compare_products']/div/div[3]/a") != null)
            {
                foreach (var htmlNode in doc.DocumentNode.SelectNodes("//*[@class='pr-line_link']"))
                {
                    if (htmlNode.Attributes["href"] != null)
                    {
                        return uri = htmlNode.Attributes["href"].Value;
                    }
                    return uri;

                }
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
            double minPrice=0,maxPrice=0;
            string imageUrl = null;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (doc.DocumentNode.SelectNodes("/html/body/div[1]/div[5]/div[2]/div[2]/div[1]/div/div[1]/div[3]/div/div[1]/div[2]/div[1]/div/div[1]/div[1]/span") != null)
            {
                foreach (var htmlNode in doc.DocumentNode.SelectNodes("/html/body/div[1]/div[5]/div[2]/div[2]/div[1]/div/div[1]/div[3]/div/div[1]/div[2]/div[1]/div/div[1]/div[1]/span"))
                {
                    if (!string.IsNullOrEmpty(htmlNode.InnerText))
                    {
                        var price = htmlNode.InnerText;
                        minPrice = double.Parse(price);
                    }
                }
            }

            if (doc.DocumentNode.SelectNodes("/html/body/div[1]/div[5]/div[2]/div[2]/div[1]/div/div[1]/div[3]/div/div[1]/div[2]/div[1]/div/div[3]/div[1]/span") != null)
            {
                foreach (var htmlNode in doc.DocumentNode.SelectNodes("/html/body/div[1]/div[5]/div[2]/div[2]/div[1]/div/div[1]/div[3]/div/div[1]/div[2]/div[1]/div/div[3]/div[1]/span"))
                {
                    if (!string.IsNullOrEmpty(htmlNode.InnerText))
                    {
                        var price = htmlNode.InnerText;
                        maxPrice = double.Parse(price);
                    }
                }
            }

            if (doc.DocumentNode.SelectNodes("//*[@id='colorbox_image_1000']/img") != null)
            {
                foreach (var htmlNode in doc.DocumentNode.SelectNodes("//*[@id='colorbox_image_1000']/img"))
                {
                    if (!string.IsNullOrEmpty(htmlNode.GetAttributeValue("src","")))
                    {
                        imageUrl = htmlNode.GetAttributeValue("src", "");
                        
                    }
                }
            }

            ProvidersProductInfo p = new ProvidersProductInfo {ProviderName = "1K",MaxPrice = maxPrice, MinPrice = minPrice, ImageUrl = imageUrl};

            return p;
        }
    }
}
