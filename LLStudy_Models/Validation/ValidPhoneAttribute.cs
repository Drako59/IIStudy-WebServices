using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    public class ValidPhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string word = value?.ToString() ?? "";
            
            if(word.Length != 10)
            {
                return false;
            }
            if (!word.StartsWith("05")) return false;
            
            return word.All(char.IsDigit);
        }
    }
}
