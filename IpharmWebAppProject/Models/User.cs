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
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //name
        [Required(ErrorMessage = "First name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed")]
        public string LastName { get; set; }

        //birthday
        [Required(ErrorMessage = "Birthday is required")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        //mobile
        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber,ErrorMessage = "Phone isn't valid")]
        public string Mobile { get; set; }

        //password
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password isn't valid")]
        public string Password { get; set; }

        //adress
        [Required(ErrorMessage = "Postal code is required")]
        [DataType(DataType.PostalCode, ErrorMessage = "Postal code isn't valid")]
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
    }
}
