using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public enum Genders { Women, Men, Unisex }

    public enum Categories {Skincare, Haircare, Makeup}

    public class Product
    {
        [Key]
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency,ErrorMessage = "Price isn't valid")]
        [Range(0.01, 1000, ErrorMessage = "Price range is between 0$ to 1000$")]
        public float Price { get; set; } //$ >0

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 999, ErrorMessage = "Amount range is between 1ml to 999ml")]
        public int Amount { get; set; } //ml >0

        //description
        [Required(ErrorMessage = "Gender is required")]
        [DataType(DataType.Text)]
        public Genders Gender { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [DataType(DataType.Text)]
        public Categories Category { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [DataType(DataType.Text)]
        public string Type { get; set; } //select by...

        [Required(ErrorMessage = "Brand is required")]
        [DataType(DataType.Text)]
        public string Brand { get; set; } //select by.....

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //star rate
        public double Rate { get; set; } = 0;

        //pictures
        [Required(ErrorMessage = "At least 1 Picture URL is required")]
        [DataType(DataType.ImageUrl)]
        public string PicUrl1 { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PicUrl2 { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PicUrl3 { get; set; }

        //activation
        [Required(ErrorMessage = "Stock is required")]
        [Range(0, 999, ErrorMessage = "Stock range is between 0 to 999 items")]
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
