using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public enum Genders { Women, Men, Unisex }

    public enum Categories {Skincare, Haircare, Makeup, Perfume}

    public class Product
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public float Price { get; set; } //$ >0

        public int Amount { get; set; } //ml >0

        //description
        public Genders Gender { get; set; }

        public Categories Category { get; set; }

        public string Type { get; set; } //select by...
        
        public string Brand { get; set; } //select by...

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //star rate
        public double Rate { get; set; } = 0;

        //pictures
        [DataType(DataType.ImageUrl)]
        public string PicUrl1 { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PicUrl2 { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PicUrl3 { get; set; }

        //activation
        public int Stock { get; set; } //>=0

        public bool Active { get; set; } = true;

        //(Products) InOrders
        public ICollection<ProductInOrder> InOrders { get; set; }

        //(Products) InWishList
        public ICollection<ProductInWishList> InWishList { get; set; }

        //reviews
        public ICollection<Review> Reviews { get; set; }
    }
}
