using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Validation
{
    public class ValidDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string dateStr)
            {
                if (DateTime.TryParseExact(
                                            dateStr,
                                            "yyyy-MM-dd",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None,
                                            out _
                                          ))
                    return true;
            }
            return false;
        }
    }
}
