using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductMS.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
      
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]  
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Category { get; set; }

        public string? CreatedByUserId { get; set; }
    }
}
