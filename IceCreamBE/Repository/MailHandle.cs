using IceCreamBE.Repository.Irepository;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;

namespace IceCreamBE.Repository
{
    public class MailHandle : IMailHandle
    {
        public IConfiguration configuration;
        public MailHandle(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public bool send(string header, string message, string Receiver)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("IceCream Shop", "configuration"));
            email.To.Add(new MailboxAddress(Receiver, Receiver));

            email.Subject = header;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(configuration["mail:smtp"], 587, false);

                    smtp.Authenticate(configuration["mail:email"], configuration["mail:key"]);

                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
