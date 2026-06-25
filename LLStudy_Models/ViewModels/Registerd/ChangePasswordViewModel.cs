using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LLStudy_Models.ViewModels
{
    public class ChangePasswordViewModel : Model
    {
        [Required]
        //[FirstLetterCapital(ErrorMessage = "Valid Password is requierd")]
        [MinLength(8, ErrorMessage = "The minimum length is '8'.")]
        public string NewPassword { get; set; }
        public string RegisteredID { get; set; }
    }
}
