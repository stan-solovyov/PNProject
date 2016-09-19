using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace NotificationApp
{
    public class PriceParser:IParser
    {
        public double Parse(string html)
        {
            string price = null;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (doc.DocumentNode.SelectNodes("//a[@class='b-offers-desc__info-price-value b-offers-desc__info-price-value_primary']") != null)
            {
                foreach (var htmlNode in doc.DocumentNode.SelectNodes("//a[@class='b-offers-desc__info-price-value b-offers-desc__info-price-value_primary']"))
                {
                    var node = htmlNode.InnerText.Trim('-');
                    string[] numbers = Regex.Split(node, @"\D+");
                    price = numbers[0] + "," + numbers[1];
                }

                double finalPrice;
                if (double.TryParse(price, out finalPrice))
                {
                    return finalPrice;
                }
            }
            return 0;
        }
    }
}
