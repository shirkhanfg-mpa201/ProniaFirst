using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
        [MinLength(3)]
        [MaxLength(512)]
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
