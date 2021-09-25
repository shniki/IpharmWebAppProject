using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public class WishList
    {
        [Key]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int Counter { get; set; }

        //Products (InWishList)
        public ICollection<ProductInWishList> Products { get; set; }
    }
}
