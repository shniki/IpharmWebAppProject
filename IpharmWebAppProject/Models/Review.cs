using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        //product
        public int ProductId { get; set; }

        //user email
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int Rate { get; set; } //1-5

    }
}
