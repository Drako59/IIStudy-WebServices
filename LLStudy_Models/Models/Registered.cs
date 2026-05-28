using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Models
{


    //public class Client: Registered
    //{
    //    string registeredID;
    //    string registeredSalt;
    //    string role;

    //    public string Role { get; set; }
    //    public string RegisteredID { get; set; }

    //    public string RegisteredSalt { get; set; }

    //}
    public class Registered: Model
    {

        string registeredID;
        string registeredSalt;
        string role;
        string imagePath;
        string password;
        string phone;
        public string RegisteredID { get; set; }

        public string RegisteredSalt { get; set; }

        [Required]

        [StringLength(maximumLength: 20,MinimumLength = 2,ErrorMessage ="Valid username is required")]
        public string UserName { get; set; }
        
        [Required]
        //[FirstLetterCapital(ErrorMessage = "Valid Password is requierd")]
        [MinLength(8,ErrorMessage ="The minimum length is '8'.")]
        public string Password { get { return password; } set { password = value;  } }
        [EmailAddress(ErrorMessage = "Valid email is required")]
        public string Email { get; set; }
        public string Role { get; set; }
        [ValidDate]
        public string Birth { get; set; }

        [ValidPhone(ErrorMessage ="Ivalid phone number. Try 05XXXXXXXX")]
        public string Phone { get; set; }

        public string ImagePath { get { return imagePath; } set { this.imagePath = value;  } }
        public bool IsBanned { get; set; }

    }
}
