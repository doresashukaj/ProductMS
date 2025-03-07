using Microsoft.AspNetCore.Identity;
using ProductMS.Enums;

namespace ProductMS.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
   
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
   





}

