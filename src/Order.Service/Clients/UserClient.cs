//using Order.Service.Features.CartItems.Dtos;


//namespace Order.Service.Clients
//{
//    public class UserClient
//    {
//        private readonly HttpClient _httpClient;
//        public UserClient(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }
//        public async Task<UserResponseDto> GetUserAsync(Guid id)
//        {
//            var response = await _httpClient.GetAsync($"/users/{id}");
//            if (!response.IsSuccessStatusCode)
//                return null;

//            return await response.Content.ReadFromJsonAsync<UserResponseDto>();
//        }
//    }
//}
