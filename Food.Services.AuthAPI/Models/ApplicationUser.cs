using Microsoft.AspNetCore.Identity;

namespace Food.Services.AuthAPI.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Name { get; set; }
    }
}
