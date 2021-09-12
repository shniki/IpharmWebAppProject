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
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        //name
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed.")]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        //birthday
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        //mobile
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        //password
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //adress
        [Required]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Country { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [DataType(DataType.MultilineText)]
        public string Adress { get; set; }

        //activation
        public Type Type { get; set; } = Type.Customer;
        public bool Active { get; set; } = true;

        //orders
        public ICollection<Order> Orders { get; set; }
    }
}
