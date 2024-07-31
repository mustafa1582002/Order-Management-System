using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Core.Entities.Identity
{
    public class Customer
    {
        [Required,MaxLength(50)]
        public string Id { get; set; }
        [Required,MaxLength(50)]
        public string Name { get; set; }
        [Required,MaxLength(100)]
        public string Email { get; set; }
        public IReadOnlyList<Order> Orders { get; set; }
    }
}
