namespace gbH60A02.Model
{
    public class JoinProductCategory
    {
        public ProductCategory ProdCat { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
