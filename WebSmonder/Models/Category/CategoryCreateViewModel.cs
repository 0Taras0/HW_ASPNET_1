using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Category
{
    public class CategoryCreateViewModel
    {
        [Display(Name = "Назва категорії")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Опис")]
        public string? Description { get; set; } = string.Empty;

        [Display(Name = "Оберіть фото")]
        public IFormFile ImageFile { get; set; } = null!;
    }
}
