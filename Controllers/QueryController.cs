using Microsoft.AspNetCore.Mvc;
using BC_Api.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

namespace BC_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;
        private readonly TokenService _tokenService;
        private readonly KCBService _kcbService;

        public QueryController(TokenService tokenService, CustomerService customerService, EmployeeService employeeService, KCBService kcbService)
        {
            _tokenService = tokenService;
            _customerService = customerService;
            _employeeService = employeeService;
            _kcbService = kcbService;
        }
        private async Task<string> GetAccessTokenAsync()
        {
            // Fetch the token from KCBService
            var (accessToken, _, _) = await _kcbService.FetchTokenAsync();
            return accessToken;
        }

        //[HttpGet("customers")]
        //public async Task<IActionResult> GetCustomers()
        //{
        //    var allCustomers = await _customerService.GetCustomersAsync();
        //    return Ok(allCustomers);
        //}
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            // Fetch the access token
            var token = await GetAccessTokenAsync();

            // Pass token to CustomerService or validate token as needed
            var allCustomers = await _customerService.GetCustomersAsync(token);
            return Ok(allCustomers);
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
        {
            var token = await GetAccessTokenAsync();

            var allEmployees = await _employeeService.GetEmployeesAsync(token);
            return Ok(allEmployees);
        }

        [HttpGet("customers/{id}")]
        public async Task<IActionResult> GetCustomerById(string id)
        {
            var token = await GetAccessTokenAsync();
            var customer = await _customerService.GetCustomerByIdAsync(id, token);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        [HttpGet("employees/{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var token = await GetAccessTokenAsync();
            var employee = await _employeeService.GetEmployeeByIdAsync(id, token);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
    }
}
