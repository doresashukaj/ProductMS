using System.ComponentModel.DataAnnotations;

namespace ProductMS.DTO
{
public class UserForLoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
