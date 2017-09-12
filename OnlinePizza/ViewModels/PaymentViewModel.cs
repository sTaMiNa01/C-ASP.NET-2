using OnlinePizza.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.ViewModels
{
    public class PaymentViewModel
    {
        public List<CartItem> Dishes { get; set; }

        public int OrderSum { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int ZipCode { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Please enter valid cardnumber")]
        public long CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{2})$", ErrorMessage = "Please enter valid month")]
        public int Month { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{4})$", ErrorMessage = "Please enter valid year")]
        public int Year { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{3})$", ErrorMessage = "Please enter valid cvc")]
        public int Cvc { get; set; }
    }
}
