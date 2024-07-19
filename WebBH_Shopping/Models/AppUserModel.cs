using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebBH_Shopping.Models
{
    public class AppUserModel : IdentityUser
    {
        public string RoleId { get; set; }
       
    }
}
