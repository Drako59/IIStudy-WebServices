using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.Models;

namespace LLStudy_Models.ViewModels
{
    public class ViewRegisteredBookPreviewModel
    {
        public ViewBookViewModel BookViewModel { get; set; }
        public bool IsOwned { get; set; }
        public bool IsInShoppingCart { get; set; }
        public bool HasPurchased {get;set;}
        public List<string> LikedReviews { get; set; }
        public List<string> DislikedReviews { get; set; }
    }
}
    