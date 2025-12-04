using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Models
{

    public class Shopping_Cart:Model
    {
        string registeredID;
        [Required]
        public string RegisteredID { get; set; }
    }
}
