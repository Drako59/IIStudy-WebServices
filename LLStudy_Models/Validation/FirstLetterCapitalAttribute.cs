using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    public class FirstLetterCapitalAttribute :ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string word = value.ToString();
            char firstLetter = word[0];
            if (!(firstLetter >= 'A' && firstLetter <= 'Z'))
                return false;
            for( int i = 0; i <  word.Length; i++)
            {
                if (word[i] < 'a' || word[i] > 'z')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
