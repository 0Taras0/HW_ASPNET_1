using WebSmonder.SMTP;

namespace WebSmonder.Interfaces
{
    public interface ISMTPService
    {
        Task<bool> SendMessageAsync(Message message);
    }
}
