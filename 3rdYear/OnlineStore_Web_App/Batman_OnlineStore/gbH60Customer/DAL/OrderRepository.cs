
using System.Text;

namespace gbH60Customer.DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _client;
        private static readonly string link = "http://localhost:29892/api/checkout/";

        public OrderRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<string?> CheckCreditCard(string? cardNum)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = $"{link}CheckCreditCard?cardNum={cardNum}";
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task Checkout(int? custId)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link;
            string json = $"{{\"custId\": {custId ?? 0}}}";
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }
    }
}
