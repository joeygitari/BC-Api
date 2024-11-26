using System.Text.Json;

namespace BC_Api.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Employee>> GetEmployeesAsync(string token)
        {
            var url = "http://jo:7048/BC240/ODataV4/Company('CRONUS%20International%20Ltd.')/EmployeeList";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonDocument.Parse(jsonString);
                var employees = jsonData.RootElement.GetProperty("value");
                return JsonSerializer.Deserialize<List<Employee>>(employees.ToString());
            }

            return new List<Employee>();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(string id, string token)
        {
            var url = $"http://jo:7048/BC240/ODataV4/Company('CRONUS%20International%20Ltd.')/EmployeeList('{id}')";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Employee>(jsonString);
            }

            return null;
        }
    }
}

public class Employee
{
    public string No { get; set; }
    public string FullName { get; set; }
    public string Phone_No { get; set; }
}   