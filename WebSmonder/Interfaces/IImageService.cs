using System.Threading.Tasks;

namespace WebSmonder.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
        Task<string> SaveImageFromUrlAsync(string imageUrl);
        Task DeleteImageAsync(string name);
    }
}
