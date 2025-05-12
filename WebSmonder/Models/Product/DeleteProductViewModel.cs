using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Product
{
    public class DeleteProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Назва продукту")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Опис:")]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Основні зображення")]
        public List<string> ProductImageNames { get; set; } = new();
    }
}
