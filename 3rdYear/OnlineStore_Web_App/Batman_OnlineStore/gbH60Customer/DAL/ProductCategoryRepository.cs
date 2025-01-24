using gbH60Customer.DTO;
using gbH60Customer.Model;
using Newtonsoft.Json;

namespace gbH60Customer.DAL
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly HttpClient _client;
        private static readonly string link = "http://localhost:29892/api/ProductCategories/";

        public ProductCategoryRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ProductCategory>> DisplayAll()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link;
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<List<ProductCategory>>(responseData);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<ProductDTO>> DisplayProductsOfCategory(int? id)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + $"products/{id ?? 0}";
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                List<Product> prods = JsonConvert.DeserializeObject<List<Product>>(responseData);
                return ProductDTO.ToList(prods);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<ProductCategory?> Find(int? id)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + (id ?? 0);
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<ProductCategory>(responseData);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ProductCategory>> FindByName(string name)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + $"name/{name}";
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<IEnumerable<ProductCategory>>(responseData);
            }
            else
            {
                return [];
            }
        }
    }
}
