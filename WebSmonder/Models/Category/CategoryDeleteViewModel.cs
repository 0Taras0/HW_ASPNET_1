using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Category
{
    public class CategoryDeleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
