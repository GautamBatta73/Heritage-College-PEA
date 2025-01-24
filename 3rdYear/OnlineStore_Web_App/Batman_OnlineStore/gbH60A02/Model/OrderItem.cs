using System.ComponentModel.DataAnnotations;

namespace gbH60A02.Model
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }

        [Required]
        public int? OrderId { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [Required]
        [Range(0, 20)]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public virtual Product? Product { get; set; } = null!;
        public virtual Order? Order { get; set; } = null!;
    }
}
