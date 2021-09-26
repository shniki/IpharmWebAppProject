using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int ReviewId { get; set; }

        //product
        [Required]
        public int ProductId { get; set; }

        //user email
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(1,5)]
        public int Rate { get; set; } //1-5

    }
}
