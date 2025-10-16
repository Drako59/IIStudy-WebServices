using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Model
{
    public class Shopping_Cart
    {
        string userName;
        [Required]
        public string UserName { get { return userName; } set { userName = value; } }
    }
}
