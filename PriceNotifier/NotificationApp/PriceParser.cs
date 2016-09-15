using PriceNotifier.DTO;

namespace NotificationApp
{
    public class PriceParser:IParser
    {
        public double Parse(ProductDto productDto)
        {
            double price;
            if (double.TryParse(productDto.Price, out price))
            {
                return price;
            }

            return 0;
        }
    }
}
