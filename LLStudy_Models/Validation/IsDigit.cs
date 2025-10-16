using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    public class IsDigits: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            char min = '0', max = '9';
            for (int i = 0; i < value.ToString().Length; i++)
            {
                if (!(value.ToString()[i] >= min && value.ToString()[i] <= max))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
