using System.Text.Json;

namespace BC_Api.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Customer>> GetCustomersAsync(string token)
        {
            var url = "http://jo:7048/BC240/ODataV4/Company('CRONUS%20International%20Ltd.')/CustomerList";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonDocument.Parse(jsonString);
                var customers = jsonData.RootElement.GetProperty("value");
                return JsonSerializer.Deserialize<List<Customer>>(customers.ToString());
            }

            return new List<Customer>();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id, string token)
        {
            var url = $"http://jo:7048/BC240/ODataV4/Company('CRONUS%20International%20Ltd.')/CustomerList('{id}')";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(jsonString);
            }

            return null;
        }
    }

    public class Customer
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Post_Code { get; set; }
        public string Shipping_Advice { get; set; }
        public string Contact { get; set; }
        public string Reserve { get; set; }
    }
}