using gbH60A02.Model;
using Newtonsoft.Json;
using System.Text;

namespace gbH60A02.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly HttpClient _client;
        private static readonly string link = "http://localhost:29892/api/customers/";

        public CustomerRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task Add(Customer c)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link;
            string jsonContent = JsonConvert.SerializeObject(c);
            StringContent content = new(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<Customer>> DisplayAll()
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

                return JsonConvert.DeserializeObject<List<Customer>>(responseData);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<Customer?> Find(int? id)
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

                return JsonConvert.DeserializeObject<Customer>(responseData);
            }
            else
            {
                return null;
            }
        }

        public async Task<Customer?> FindByEmail(string email)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + $"email/{email}";
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<Customer>(responseData);
            }
            else
            {
                return null;
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

        public async Task Update(Customer c)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + c.CustomerId;
            string jsonContent = JsonConvert.SerializeObject(c);
            StringContent content = new(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }
    }
}
