using System.ComponentModel.DataAnnotations;

namespace gbH60A02.Model
{
    public partial class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int? CartId { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [Required]
        [Range(0, 20)]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public virtual Product? Product { get; set; } = null!;
        public virtual ShoppingCart? ShoppingCart { get; set; } = null!;
    }
}
