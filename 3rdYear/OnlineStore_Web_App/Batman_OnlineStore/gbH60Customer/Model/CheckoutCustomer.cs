using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace gbH60Customer.Model
{
    public class CheckoutCustomer
    {
        [Required]
        [StringLength(20)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [StringLength(30)]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [RegularExpression(@"^[\d]{10}$", ErrorMessage = "Phone Number must be in the format: '1234567890'.")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 10)]
        public string Address { get; set; }

        public string CreditCard { get; set; }

        public CheckoutCustomer()
        {
        }

        public CheckoutCustomer(Customer c)
        {
            FirstName = c.FirstName;
            LastName = c.LastName;
            Email = c.Email;
            Address = "";
            PhoneNumber = c.PhoneNumber;
            CreditCard = c.CreditCard;
        }
    }
}
