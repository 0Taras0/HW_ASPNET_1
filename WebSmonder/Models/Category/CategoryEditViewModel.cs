using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Category
{
    public class CategoryEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва категорії")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Опис")]
        public string? Description { get; set; } = string.Empty;
        [Display(Name = "Url адреса фото")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
