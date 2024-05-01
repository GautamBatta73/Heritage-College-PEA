using System.ComponentModel.DataAnnotations;

namespace HVK.Models
{
    public class Login
    {

        [Required]
        [Display(Name = "Email Address:")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "The Email Address May Not Contain More Than 50 Characters!")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address Format")]
        public string? UserEmail { get; set; }

        [Required]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Incorrect Password.")]
        public string? UserPassword { get; set; }

        public Login()
        {
            UserEmail = null;
            UserPassword = null;
        }

        public Login(string username, string password)
        {
            // "this." notation sremoves the ambiguity in between local and instance variables,
            // Instantly clears the previous value assigned avoiding cached values
            // amongst many other things that could go wrong.

            this.UserEmail = username;
            this.UserPassword = password;
        }
    }
}
