using System.Net.Mail;

namespace NotificationApp.Services
{
    public class MailService
    {
        readonly MailMessage _mail = new MailMessage();
        public void ProductAvailable(string email,string productUrl,string productName,double priceFromSite)
        {
            _mail.From = new MailAddress("price_notifier@gmail.com");
            _mail.To.Add(email);
            _mail.IsBodyHtml = true;
            SmtpClient smtpServer = new SmtpClient();
            _mail.Subject = "Product is in stock";
            _mail.Body = $"The item <a href = '{productUrl}'> {productName} </a>  is available for purchase for {priceFromSite} BYN.";
            smtpServer.Send(_mail);
        }

        public void PriceFromDbHigher(string email, string productUrl, string productName, double priceFromDb,double priceFromSite)
        {
            _mail.From = new MailAddress("price_notifier@gmail.com");
            _mail.To.Add(email);
            _mail.IsBodyHtml = true;
            SmtpClient smtpServer = new SmtpClient();
            _mail.Subject = "Price drop";
            _mail.Body = $"The price for <a href = '{productUrl}'> {productName} </a>  has decreased on {priceFromDb - priceFromSite:N} BYN.";
            smtpServer.Send(_mail);
        }

        public void PriceFromSiteHigher(string email, string productUrl, string productName, double priceFromDb, double priceFromSite)
        {
            _mail.From = new MailAddress("price_notifier@gmail.com");
            _mail.To.Add(email);
            _mail.IsBodyHtml = true;
            SmtpClient smtpServer = new SmtpClient();
            _mail.Subject = "Price increase";
            _mail.Body = $"The price for <a href = '{productUrl}'> {productName} </a>  has increased on {priceFromSite - priceFromDb:N} BYN.";
            smtpServer.Send(_mail);
        }

        public void ProductOutOfStock(string email, string productUrl, string productName)
        {
            _mail.From = new MailAddress("price_notifier@gmail.com");
            _mail.To.Add(email);
            _mail.IsBodyHtml = true;
            SmtpClient smtpServer = new SmtpClient();
            _mail.Subject = "Product is out of sale";
            _mail.Body = $"Unfortunately the <a href = '{productUrl}'> {productName} </a>  is out of sale.";
            smtpServer.Send(_mail);
        }

    }
}
