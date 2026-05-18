using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Models;

namespace LLStudy_Models.ViewModels
{
    public class SignInViewModel : Model
    {
        private string signKey;
        private string password;

        [Required(ErrorMessage = "The Identifier filed is required.")]
        public string SignKey { get { return this.signKey; } set { this.signKey = value; ValidateProperty(value, nameof(this.SignKey)); } }
        [Required]
        public string Password { get { return this.password; } set { this.password = value; ValidateProperty(value, nameof(this.Password)); } }

    }
}
