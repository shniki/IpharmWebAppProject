using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpharmWebAppProject.Models
{
    public class ProductInOrder
    {
        public int ProductInOrderId { get; set; }
        public int Amount { get; set; } = 1; //>0

        //order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        //product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
