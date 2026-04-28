using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    public class PostalAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string postal = value?.ToString() ?? "";
            if (postal.Count() != 7)
            {
                return false;
            }

            foreach(char chr in postal)
            {
                if (chr > '9' || chr < '0') return false;
            }
            return true;
        }
    }
}
