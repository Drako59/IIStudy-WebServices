using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Models
{
    public class Registered: Model
    {
        string userName;
        string password;
        string email;
        string role;
        string birth;
        [Required]
        [StringLength(maximumLength: 20,MinimumLength =2,ErrorMessage ="Valid username is required")]
        public string UserName { get; set; }
        
        [Required]
        [FirstLetterCapital(ErrorMessage ="Valid Password is requierd")]
        [StringLength(maximumLength: 20, MinimumLength = 8, ErrorMessage = "Valid username is required")]
        public string Password { get; set; }
        [EmailAddress(ErrorMessage = "Valid email is required")]
        public string Email { get; set; }
        [LegalRole(ErrorMessage ="Valid role is required")]
        public string Role { get; set; }
        public string Birth { get; set; }

    }
}
