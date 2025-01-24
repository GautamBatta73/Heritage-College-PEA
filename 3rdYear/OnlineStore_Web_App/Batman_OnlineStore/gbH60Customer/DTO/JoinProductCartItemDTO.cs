using gbH60Customer.Model;

namespace gbH60Customer.DTO
{
    public class JoinProductCartItemDTO
    {
        public ProductDTO Product { get; set; }
        public CartItem CartItem { get; set; }

        public JoinProductCartItemDTO() { }

        public JoinProductCartItemDTO(JoinProductCartItem c)
        {
            CartItem = c.CartItem;
            Product = new ProductDTO(c.Product);
        }

        public static IEnumerable<JoinProductCartItemDTO> ToList(List<JoinProductCartItem> p)
        {
            List<JoinProductCartItemDTO> list = [];

            p.ForEach(x => list.Add(new JoinProductCartItemDTO(x)));

            return list;
        }
    }
}
