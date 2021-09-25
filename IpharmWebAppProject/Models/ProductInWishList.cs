using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpharmWebAppProject.Models
{
    public class ProductInWishList
    {
        public int ProductInWishListId { get; set; }

        //order
        public string WishListId { get; set; }
        public WishList WishList { get; set; }

        //product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
