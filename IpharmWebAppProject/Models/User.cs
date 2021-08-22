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
        public string Email { get; set; }

        //name
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //birthday
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        //mobile
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        //password
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //adress
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        [DataType(DataType.MultilineText)]
        public string Adress { get; set; }

        //activation
        public Type Type { get; set; }
        public bool Active { get; set; } = true;

        //orders
        public ICollection<Order> Orders { get; set; }
    }
}
