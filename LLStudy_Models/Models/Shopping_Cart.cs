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
        [Required]
        public string RegisteredID { get; set; }
        public string BookID { get; set; }

        public int CountBooks { get; set; }
    }
}
