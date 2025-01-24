using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using gbH60Customer.Model;

namespace gbH60Customer.DTO;

public class ProductDTO
{
    public int ProductId { get; set; }

    public int ProdCatId { get; set; }

    [DisplayName("Product Name")]
    public string Name { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Product Description")]
    public string? Description { get; set; }

    [DisplayName("Image URL")]
    public string? ImageURL { get; set; }

    [DisplayName("Manufacturer")]
    public string? Manufacturer { get; set; }

    [Range(0, 99)]
    public int Stock { get; set; }

    [DataType(DataType.Currency)]
    [DisplayName("Price")]
    public decimal? SellPrice { get; set; }

    public ProductDTO()
    {
    }

    public ProductDTO(Product p)
    {
        ProductId = p.ProductId;
        ProdCatId = p.ProdCatId;
        Name = p.Name;
        Description = p.Description;
        ImageURL = p.ImageURL;
        Manufacturer = p.Manufacturer;
        Stock = p.Stock;
        SellPrice = p.SellPrice;
    }

    public static IEnumerable<ProductDTO> ToList(List<Product> p)
    {
        List<ProductDTO> list = [];

        p.ForEach(x => list.Add(new ProductDTO(x)));

        return list;
    }
}
