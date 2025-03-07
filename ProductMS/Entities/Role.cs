using Microsoft.AspNetCore.Identity;

namespace ProductMS.Entities
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
