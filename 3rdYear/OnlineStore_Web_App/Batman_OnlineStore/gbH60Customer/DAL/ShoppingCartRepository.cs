using gbH60Customer.DTO;
using gbH60Customer.Model;
using Newtonsoft.Json;
using System.Text;

namespace gbH60Customer.DAL
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly HttpClient _client;
        private static readonly string link = "http://localhost:29892/api/shoppingcart/";

        public ShoppingCartRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task AddToCart(int? prodId, int? custId)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + "addtocart";
            string json = $"{{\"prodId\": {(prodId ?? 0)}, \"custId\": {(custId ?? 0)} }}";
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public async Task RemoveFromCart(int? prodId, int? custId)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + "removefromcart";
            string json = $"{{\"prodId\": {(prodId ?? 0)}, \"custId\": {(custId ?? 0)} }}";
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<JoinProductCartItemDTO>> GetCartItems(int? custId)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + $"getitems/{(custId ?? 0)}";
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                List<JoinProductCartItem> items = JsonConvert.DeserializeObject<List<JoinProductCartItem>>(responseData);
                return JoinProductCartItemDTO.ToList(items);
            }
            else
            {
                return [];
            }
        }

        public async Task RemoveSingle(int? prodId, int? custId)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + "removesingle";
            string json = $"{{\"prodId\": {prodId}, \"custId\": {custId} }}";
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }
    }
}
