using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Core.Entities
{
    public class Product:BaseEntity
    {
        [Required,MaxLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Required,MaxLength(200)]
        public int Stock { get; set; }
    }
}
