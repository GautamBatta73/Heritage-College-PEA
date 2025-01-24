using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace gbH60Customer.Model
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public enum Provinces
        {
            MB,
            NB,
            ON,
            QC
        }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z'\s-]*$", ErrorMessage = "First Name can only contain letters, apstrophes, dashes, and spaces.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z'\s-]*$", ErrorMessage = "Last Name can only contain letters, apstrophes, dashes, and spaces.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30)]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[\d]{10}$", ErrorMessage = "Phone Number must be in the format: '1234567890'.")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-z]{2}$", ErrorMessage = "Province must be valid.")]
        public string Province { get; set; }

        [Required]
        [StringLength(16)]
        [RegularExpression(@"^[\d]+$", ErrorMessage = "Credit Card Number must be all numbers.")]
        [DisplayName("Credit Card Number")]
        public string CreditCard { get; set; }

        public virtual ShoppingCart? ShopCart { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
