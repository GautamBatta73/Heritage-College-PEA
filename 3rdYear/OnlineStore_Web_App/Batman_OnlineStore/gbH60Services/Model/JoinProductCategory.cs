namespace gbH60Services.Model
{
    public class JoinProductCategory
    {
        public ProductCategory ProdCat { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
