using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBH_Shopping.Models
{
    public class RolesModel : IdentityRole
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
}
