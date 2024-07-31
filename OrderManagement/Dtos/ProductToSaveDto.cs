using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Api.Dtos
{
    public class ProductToSaveDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Range(0,int.MaxValue)]
        public decimal Price { get; set; }
        [Required, Range(0,int.MaxValue)]
        public int Stock { get; set; }
    }

}
