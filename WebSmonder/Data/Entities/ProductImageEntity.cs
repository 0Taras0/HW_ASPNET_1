using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Data.Entities
{
    [Table("tbl_productImages")]
    public class ProductImageEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        public int Priotity { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
        //public object ImageUrl { get; internal set; }
    }
}
