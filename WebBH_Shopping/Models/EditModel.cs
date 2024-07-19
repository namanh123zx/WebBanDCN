using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebBH_Shopping.Models
{
    public class EditModel 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Role { get; set; }

        
    }
}
