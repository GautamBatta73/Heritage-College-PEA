using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace gbH60Customer.Model;

public partial class ProductCategory
{
    public int CategoryId { get; set; }

    [Required]
    [StringLength(60)]
    [DisplayName("Category")]
    public string ProdCat { get; set; } = null!;

    [StringLength(2083)]
    [DisplayName("Image URL")]
    public string? ImageURL { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
