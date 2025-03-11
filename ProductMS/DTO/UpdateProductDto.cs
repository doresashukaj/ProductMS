using System.ComponentModel.DataAnnotations;

namespace ProductMS.DTO
{
    public class UpdateProductDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        public string? Category { get; set; }

     
    }
}
