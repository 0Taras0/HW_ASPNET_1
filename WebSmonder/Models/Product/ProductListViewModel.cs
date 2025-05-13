using WebSmonder.Models.Helpers;

namespace WebSmonder.Models.Product
{
    public class ProductListViewModel
    {
        //Відображаємо список продуктів
        public List<ProductItemViewModel> Products { get; set; } = new();
        //Модель для пошуку
        public ProductSearchViewModel Search { get; set; } = new();
        //Кількість елементів під час пошуку
        public int Count { get; set; }
    }
}
