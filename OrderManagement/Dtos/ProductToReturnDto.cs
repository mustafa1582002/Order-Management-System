using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Api.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Required, MaxLength(200)]
        public string Stock { get; set; }
    }
}
