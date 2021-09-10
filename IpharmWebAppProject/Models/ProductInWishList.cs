using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpharmWebAppProject.Models
{
    public class ProductInWishList
    {
        public int ProductInWishListID { get; set; }

        //order
        public string WishListID { get; set; }
        public WishList WishList { get; set; }

        //product
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
