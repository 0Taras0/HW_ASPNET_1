using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Data.Entities
{
    [Table("tbl_products")]
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(500)]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(40000)]
        public string Description { get; set; } = string.Empty;
        public ICollection<string> DescriptionImages { get; set; } = new List<string>();

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public CategoryEntity? Category { get; set; }
        public ICollection<ProductImageEntity>? ProductImages { get; set; }
    }
}
