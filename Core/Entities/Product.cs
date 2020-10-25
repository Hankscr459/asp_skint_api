using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        [Required]
        public int? ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        [Required]
        public int? ProductBrandId { get; set; }
    }
}