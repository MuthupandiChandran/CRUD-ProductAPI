using Microsoft.AspNetCore.Identity;

namespace ProductAPI.DTO
{
    public class ApplicationUser:IdentityUser
    {
        public string Role { get; set; }
    }
}
