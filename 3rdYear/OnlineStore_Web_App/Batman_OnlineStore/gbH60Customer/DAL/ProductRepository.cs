using gbH60Customer.DTO;
using gbH60Customer.Model;
using Newtonsoft.Json;

namespace gbH60Customer.DAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _client;
        private readonly IProductCategoryRepository _catRepo;
        private static readonly string link = "http://localhost:29892/api/products/";

        public ProductRepository(HttpClient client, IProductCategoryRepository catRepo)
        {
            _client = client;
            _catRepo = catRepo;
        }

        public async Task<IEnumerable<ProductDTO>> DisplayAll()
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

                List<Product> prods = JsonConvert.DeserializeObject<List<Product>>(responseData);
                return ProductDTO.ToList(prods);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<JoinProductCategoryDTO>> DisplayByCategory()
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

                List<JoinProductCategory> prods = JsonConvert.DeserializeObject<List<JoinProductCategory>>(responseData);
                return JoinProductCategoryDTO.ToList(prods);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<ProductDTO?> Find(int? id)
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

                Product prod = JsonConvert.DeserializeObject<Product>(responseData);
                return new ProductDTO(prod);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<JoinProductCategoryDTO>> FindByName(string name)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            string endpoint = link + $"name/{name}";
            IEnumerable<Product> prodList = [];
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                string responseData = await reader.ReadToEndAsync();

                prodList = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseData);
            }
            else
            {
                return [];
            }

            if (prodList == null)
            {
                return [];
            }

            var groupedProducts = (await Task.WhenAll(
                prodList.GroupBy(p => new { p.ProdCatId })
                .Select(async group => new JoinProductCategory
                {
                    ProdCat = await _catRepo.Find(group.Key.ProdCatId),
                    Products = group.OrderBy(x => x.Name).ToList()
                })
            )).OrderBy(x => x.ProdCat.ProdCat)
                .ToList();

            return JoinProductCategoryDTO.ToList(groupedProducts);
        }
    }
}
