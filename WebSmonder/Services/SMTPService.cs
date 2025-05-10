using MailKit.Net.Smtp;
using MimeKit;
using WebSmonder.Interfaces;
using WebSmonder.SMTP;

namespace WebSmonder.Services
{
    public class SMTPService : ISMTPService
    {

        public async Task<bool> SendMessageAsync(Message message)
        {
            //EmailConfiguration config = new EmailConfiguration();
            //string pathFile = @"D:\ss.webp";

            //var attachment = new MimePart("image", "webp")
            //{
            //    FileName = "Привіт друже",
            //    Content = new MimeContent(File.OpenRead(pathFile))
            //};
            var body = new TextPart("html")
            {
                Text = message.Body
            };
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            //multipart.Add(attachment);


            // Створення повідомлення
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(EmailConfiguration.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;

            // Тіло повідомлення
            emailMessage.Body = multipart;

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(EmailConfiguration.SmtpServer, EmailConfiguration.Port, true);
                await client.AuthenticateAsync(EmailConfiguration.UserName, EmailConfiguration.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error send EMAIL {0}", ex.Message);
            }
            return false;
        }
    }
}
