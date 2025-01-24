using System.ComponentModel.DataAnnotations;

namespace gbH60A02.Model
{
    public partial class ShoppingCart
    {
        public int CartId { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [Required]
        public DateTime? DateCreated { get; set; }

        public virtual Customer? Customer { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
