using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.ViewModels
{
    public class UpdateProfileResult
    {
        public bool EmailIsTaken { get; set; }
        public bool UserNameIsTaken { get; set; }
        public bool Success { get; set; }

    }
}
