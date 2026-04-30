using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    public class ValidFile : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            List<string> validExt = new List<string>() { ".pdf" };
            if (value != null && value.ToString().ToLower() == "none")
                return true;
            string fileName = Path.GetFileName(value?.ToString() ?? "");
            string ext = Path.GetExtension(fileName);

            if (validExt.Contains(ext.ToLower())) return true;
            return false;
        }
    }
}
