using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
        [Display(Name = "Street")]
        [StringLength(100)]
        public string Street { get; set; }

        [Display(Name = "Zipcode")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int Zipcode { get; set; }

        [Display(Name = "City")]
        [StringLength(100)]
        public string City { get; set; }

    }
}
