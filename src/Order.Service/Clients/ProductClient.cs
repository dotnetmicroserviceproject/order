//using Order.Service.Entities;

//namespace Order.Service.Clients
//{
//    public class ProductClient
//    {
//        private readonly HttpClient _httpClient;

//        public ProductClient(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<ProductItem> GetProductAsync(Guid id)
//        {
//            var response = await _httpClient.GetAsync($"/products/{id}");
//            if (!response.IsSuccessStatusCode)
//                return null;

//            return await response.Content.ReadFromJsonAsync<ProductItem>();
//        }

      
//    }
//}
