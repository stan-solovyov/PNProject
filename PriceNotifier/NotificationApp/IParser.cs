using PriceNotifier.DTO;

namespace NotificationApp
{
    public interface IParser
    {
        double Parse(string page);
    }
}
