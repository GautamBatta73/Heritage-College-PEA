using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace gbH60A02.Model;

public partial class Product
{
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a valid category.")]
    public int ProdCatId { get; set; }

    [Required]
    [StringLength(50)]
    [DisplayName("Product Name")]
    public string Name { get; set; }

    [StringLength(200)]
    [DataType(DataType.MultilineText)]
    [DisplayName("Product Description")]
    public string? Description { get; set; }

    [StringLength(2083)]
    [DisplayName("Image URL")]
    public string? ImageURL { get; set; }

    [StringLength(80)]
    [DisplayName("Product Manufacturer")]
    public string? Manufacturer { get; set; }

    [Required]
    [Range(0, 99)]
    public int Stock { get; set; }

    [DataType(DataType.Currency)]
    [Range(1, 999.99)]
    [DisplayName("Buy Price")]
    public decimal? BuyPrice { get; set; }

    [DataType(DataType.Currency)]
    [Range(1, 999.99)]
    [DisplayName("Sales Price")]
    [CheckPrice(BuyPriceVal = "BuyPrice")]
    public decimal? SellPrice { get; set; }

    public virtual ProductCategory? ProdCat { get; set; } = null!;
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
