using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using NotificationApp.Interfaces;

namespace NotificationApp.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpServer = new SmtpClient();

        public async Task ProductAvailable(List<string> emails, string productUrl, string productName, double priceFromSite)
        {
            foreach (var email in emails)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("price_notifier@gmail.com");
                mail.IsBodyHtml = true;
                mail.Subject = "Product is in stock";
                mail.Body = $"The item <a href = '{productUrl}'> {productName} </a>  is available for purchase for {priceFromSite} BYN.";
                mail.To.Add(email);
                await _smtpServer.SendMailAsync(mail);
            }
        }

        public async Task PriceFromDbHigher(List<string> emails, string productUrl, string productName, double priceFromDb, double priceFromSite)
        {
            foreach (var email in emails)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("price_notifier@gmail.com");
                mail.IsBodyHtml = true;
                mail.Subject = "Price drop";
                mail.Body = $"The price for <a href = '{productUrl}'> {productName} </a>  has decreased on {priceFromDb - priceFromSite:N} BYN.";
                mail.To.Add(email);
                await _smtpServer.SendMailAsync(mail);
            }
        }

        public async Task PriceFromSiteHigher(List<string> emails, string productUrl, string productName, double priceFromDb, double priceFromSite)
        {
            foreach (var email in emails)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("price_notifier@gmail.com");
                mail.IsBodyHtml = true;
                mail.Subject = "Price increase";
                mail.Body = $"The price for <a href = '{productUrl}'> {productName} </a>  has increased on {priceFromSite - priceFromDb:N} BYN.";
                mail.To.Add(email);
                await _smtpServer.SendMailAsync(mail);
            }
        }

        public async Task ProductOutOfStock(List<string> emails, string productUrl, string productName)
        {
            foreach (var email in emails)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("price_notifier@gmail.com");
                mail.IsBodyHtml = true;
                mail.Subject = "Product is out of sale";
                mail.Body = $"Unfortunately the <a href = '{productUrl}'> {productName} </a>  is out of sale.";
                mail.To.Add(email);
                await _smtpServer.SendMailAsync(mail);
            }
        }
    }
}
