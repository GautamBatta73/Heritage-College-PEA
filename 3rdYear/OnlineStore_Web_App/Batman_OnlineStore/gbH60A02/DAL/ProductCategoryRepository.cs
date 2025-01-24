using gbH60A02.Model;
using Newtonsoft.Json;
using System.Text;

namespace gbH60A02.DAL
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly HttpClient _client;
        private static readonly string link = "http://localhost:29892/api/ProductCategories/";

        public ProductCategoryRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task Add(ProductCategory p)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link;
            string jsonContent = JsonConvert.SerializeObject(p);
            StringContent content = new(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
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

        public async Task<IEnumerable<Product>> DisplayProductsOfCategory(int? id)
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

                return JsonConvert.DeserializeObject<List<Product>>(responseData);
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

        public async Task Remove(int? id)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + (id ?? 0);
            HttpResponseMessage response = await _client.DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public async Task Update(ProductCategory p)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + p.CategoryId;
            string jsonContent = JsonConvert.SerializeObject(p);
            StringContent content = new(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }
    }
}
