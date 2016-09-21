using System.Net.Mail;

namespace NotificationApp.Services
{
    public class MailService
    {
        readonly SmtpClient _smtpServer = new SmtpClient();
        public void ProductAvailable(string email,string productUrl,string productName,double priceFromSite)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("price_notifier@gmail.com");
            mail.To.Add(email);
            mail.IsBodyHtml = true;
            mail.Subject = "Product is in stock";
            mail.Body = $"The item <a href = '{productUrl}'> {productName} </a>  is available for purchase for {priceFromSite} BYN.";
            _smtpServer.Send(mail);
        }

        public void PriceFromDbHigher(string email, string productUrl, string productName, double priceFromDb,double priceFromSite)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("price_notifier@gmail.com");
            mail.To.Add(email);
            mail.IsBodyHtml = true;
            mail.Subject = "Price drop";
            mail.Body = $"The price for <a href = '{productUrl}'> {productName} </a>  has decreased on {priceFromDb - priceFromSite:N} BYN.";
            _smtpServer.Send(mail);
        }

        public void PriceFromSiteHigher(string email, string productUrl, string productName, double priceFromDb, double priceFromSite)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("price_notifier@gmail.com");
            mail.To.Add(email);
            mail.IsBodyHtml = true;
            mail.Subject = "Price increase";
            mail.Body = $"The price for <a href = '{productUrl}'> {productName} </a>  has increased on {priceFromSite - priceFromDb:N} BYN.";
            _smtpServer.Send(mail);
        }

        public void ProductOutOfStock(string email, string productUrl, string productName)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("price_notifier@gmail.com");
            mail.To.Add(email);
            mail.IsBodyHtml = true;
            mail.Subject = "Product is out of sale";
            mail.Body = $"Unfortunately the <a href = '{productUrl}'> {productName} </a>  is out of sale.";
            _smtpServer.Send(mail);
        }

    }
}
