using gbH60A02.Model;
using Newtonsoft.Json;
using System.Text;

namespace gbH60A02.DAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _client;
        private static readonly string link = "http://localhost:29892/api/products/";

        public ProductRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task Add(Product p)
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

        public async Task<IEnumerable<Product>> DisplayAll()
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

                return JsonConvert.DeserializeObject<List<Product>>(responseData);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<JoinProductCategory>> DisplayByCategory()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + "GetWithCategory";
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<List<JoinProductCategory>>(responseData);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<Product?> Find(int? id)
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

                return JsonConvert.DeserializeObject<Product>(responseData);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> FindByName(string name)
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

                return JsonConvert.DeserializeObject<IEnumerable<Product>>(responseData);
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

        public async Task Update(Product p)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + p.ProductId;
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
