using Microsoft.AspNetCore.Identity;

namespace BuildingShop.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
