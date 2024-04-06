using Microsoft.AspNetCore.Identity;


namespace WebsiteBanTraSua.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
    }
}
