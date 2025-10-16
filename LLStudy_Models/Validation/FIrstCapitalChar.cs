using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    internal class FIrstCapitalChar: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string word = value.ToString();
            char firstLetter = word[0];
            if (!(firstLetter >= 'A' && firstLetter <= 'Z'))
                return true;
           
            return false;
        }
    }
}
