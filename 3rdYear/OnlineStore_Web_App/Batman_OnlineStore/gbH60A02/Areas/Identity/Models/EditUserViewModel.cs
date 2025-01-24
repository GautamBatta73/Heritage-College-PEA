using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace gbH60A02.Areas.Identity.Models
{
    public class EditUserViewModel
    {
        public IdentityUser User { get; set; }
        public string UserRole { get; set; }
        public List<string> AvailableRoles { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }


}
