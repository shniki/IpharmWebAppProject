using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public enum Status {Cart, Paid, Arrived}
    public class Order
    {
        public int OrderID { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public Status Status { get; set; } = Status.Cart;

        public float Price { get; set; } = 0; //$ >=0

        public DateTime OrderDate { get; set; } //today, automatic

        //Products (InOrders)
        public ICollection<ProductInOrder> Products { get; set; }
    }
}
