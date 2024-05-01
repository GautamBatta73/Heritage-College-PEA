using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HVK.Models
{
    public partial class Hvkuser
    {
        public Hvkuser()
        {
            Pets = new HashSet<Pet>();
        }

        public int HvkuserId { get; set; }

        [Display(Name = "First Name:")]
        [DataType(DataType.Text)]
        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The First Name Must Be In Between 3 and 25 Characters.")]
        [RegularExpression("^[A-Za-z][A-Za-z\\-\\']+$", ErrorMessage = "The First Name May Only Contain Letters, Hyphens and Apostrophees.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name:")]
        [DataType(DataType.Text)]
        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The Last Name Must Be In Between 3 and 25 Characters.")]
        [RegularExpression("^[A-Za-z][A-Za-z\\-\\'\\s]+$", ErrorMessage = "The Last Name Must Start With a Capital Letter and May Only Contain Letters, Hyphens and Apostrophees, and Spaces.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Email Address:")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "The Email Address May Only Contain 50 Characters")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Password must be between 1 and 50 characters.")]
        public string? Password { get; set; }

        [Display(Name = "Street Name:")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Street Name Must Be In Between 3 and 40 Characters.")]
        [RegularExpression("^[0-9A-Za-z\\s'-]+$", ErrorMessage = "The Street Name May Only Contain Numbers, Letters, Dashes and Apostrophes, and Spaces.")]
        public string? Street { get; set; }

        [Display(Name = "City:")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The City Name Must Be In Between 3 and 50 Characters.")]
        public string? City { get; set; }

        [Display(Name = "Province:")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "The Province Name Must Be 2 characters.")]
        public string? Province { get; set; }

        [Display(Name = "Postal Code:")]
        [MaxLength(6, ErrorMessage = "The Postal Code May Only Contain 6 Characters.")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z]\d[A-Za-z]\d$", ErrorMessage = "Must follow Canadian Postal Code Standards, eg: A1A1A1")]
        public string? PostalCode { get; set; }

        [Display(Name = "Home Phone Number:")]
        [DataType(DataType.Text)]
        [RegularExpression("^\\(?(\\d{3})\\)?[- ]?(\\d{3})[- ]?(\\d{4})$", ErrorMessage = "The Phone Number May Only Be Written In The Following Formats: \"1234567890\", \"123-456-7890\", \"(123) 456-7890\".")]
        public string? Phone { get; set; }

        [Display(Name = "Cell-Phone Number:")]
        [DataType(DataType.Text)]
        [RegularExpression("^\\(?(\\d{3})\\)?[- ]?(\\d{3})[- ]?(\\d{4})$", ErrorMessage = "The Phone Number May Only Be Written In The Following Formats: \"1234567890\", \"123-456-7890\", \"(123) 456-7890\".")]
        public string? CellPhone { get; set; }

        [Display(Name = "Emergency Contact First Name:")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The First Name Must Be In Between 3 and 25 Characters.")]
        [RegularExpression("^[A-Za-z][A-Za-z\\-\\']+$", ErrorMessage = "The First Name May Only Contain Letters, Hyphens and Apostrophees.")]
        public string? EmergencyContactFirstName { get; set; }

        [Display(Name = "Emergency Contact Last Name:")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The Last Name Must Be In Between 3 and 25 Characters.")]
        [RegularExpression("^[A-Za-z][A-Za-z\\-\\'\\s]+$", ErrorMessage = "The Last Name Must Start With a Capital Letter and May Only Contain Letters, Hyphens and Apostrophees, and Spaces.")]
        public string? EmergencyContactLastName { get; set; }

        [Display(Name = "Emergency Contact Phone Number:")]
        [DataType(DataType.Text)]
        [RegularExpression("^\\(?(\\d{3})\\)?[- ]?(\\d{3})[- ]?(\\d{4})$", ErrorMessage = "The Phone Number May Only Be Written In The Following Formats: \"1234567890\", \"123-456-7890\", \"(123) 456-7890\".")]
        public string? EmergencyContactPhone { get; set; }
        public string UserType { get; set; } = null!;

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
