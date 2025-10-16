using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    public class LegalRole: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string[] Roles = { "Admin", "User", "Guest" };
            for (int i = 0; i < Roles.Length; i++)
            {
                if (Roles[i] == value.ToString())
                    return true;
            }

            return false;
        }
    }
}
