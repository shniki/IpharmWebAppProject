using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public enum Type {Manager, Customer}
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress,ErrorMessage ="please enter a valid email")]
      [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage ="Email is'nt valid")]
        public string Email { get; set; }

        //name
        [Required(ErrorMessage = "First name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed")]
        [MinLength(2, ErrorMessage = "Name is'nt valid")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed")]
        [MinLength(2,ErrorMessage ="Name is'nt valid")]
        public string LastName { get; set; }

        //birthday
        [Required(ErrorMessage = "Birthday is required")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        //mobile
        [Required(ErrorMessage = "Mobile is required")]
        //[DataType(DataType.PhoneNumber,ErrorMessage = "Mobile isn't valid")]
        //[RegularExpression(@"^(\0 ? 1\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{ 4}$",ErrorMessage = "Mobile isn't valid")]
        public string Mobile { get; set; }

        //password
        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Your password is too short")]
        [DataType(DataType.Password, ErrorMessage = "Password isn't valid")]
        public string Password { get; set; }

        //adress
        [Required(ErrorMessage = "Postal code is required")]
        [RegularExpression(@"\d{7}$",ErrorMessage ="potal code isn't valid")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [DataType(DataType.Text,ErrorMessage = "Country isn't valid")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [DataType(DataType.Text, ErrorMessage = "City isn't valid")]
        public string City { get; set; }

        [Required(ErrorMessage = "Adress is required")]
        [DataType(DataType.MultilineText, ErrorMessage = "Adress isn't valid")]
        public string Adress { get; set; }

        //activation
        public Type Type { get; set; } = Type.Customer;
        public bool Active { get; set; } = true;

        //orders
        public ICollection<Order> Orders { get; set; }

        //orders
        public ICollection<Review> Reviews { get; set; }
    }
}
