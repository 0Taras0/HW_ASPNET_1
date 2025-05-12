using System.ComponentModel.DataAnnotations;
using WebSmonder.Models.Helpers;

namespace WebSmonder.Models.Product
{
    public class ProductSearchViewModel
    {
        [Display(Name = "Назва")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Опис")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        public List<SelectItemViewModel> Categories { get; set; } = new();
    }
}
