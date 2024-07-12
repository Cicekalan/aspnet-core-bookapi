using Microsoft.AspNetCore.Identity;

namespace BookApi.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        
    }
}