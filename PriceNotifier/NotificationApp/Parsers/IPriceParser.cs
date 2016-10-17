namespace NotificationApp.Parsers
{
    public interface IPriceParser
    {
        double? Parse(string page);
    }
}
