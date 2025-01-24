using System.ComponentModel.DataAnnotations;

namespace gbH60Services.Model
{
    public partial class Order
    {
        public int OrderId { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [Required]
        public DateTime? DateCreated { get; set; }

        public DateTime? DateFulfilled { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Taxes { get; set; }

        public virtual Customer? Customer { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
