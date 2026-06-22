using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLStudy_Models.Models
{
    public class Like : Model
    {
        public string LikeID { get; set; }
        [Required]
        public string ReviewID { get; set; }
        [Required]
        public string RegisteredID { get; set; }
        public bool IsLike { get; set; }
        public bool IsDislike { get; set; }

    }
}
