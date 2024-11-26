using Microsoft.AspNetCore.Mvc;
using BC_Api.Services;
using BC_Api.Interfaces;
using BC_Api.Token;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BC_Api.Data;

namespace BC_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SeminarController : ControllerBase
    {
        private readonly SeminarService _seminarService;
        private readonly ISeminar _seminar;
        private readonly AppDbContext dBContext;
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;
        // Constructor
        public SeminarController(SeminarService seminarService, ISeminar seminar, AppDbContext dbContext, CustomerService customerService, EmployeeService employeeService)
        {
            _seminarService = seminarService;
            _seminar = seminar;
            this.dBContext = dbContext;
            _customerService = customerService;
            _employeeService = employeeService;
        }
        
        [HttpPost("posttobc")]
        public async Task<IActionResult> PostDataToBc(SeminarData seminarData)
        {
            var response = await _seminar.PostData(seminarData);
            return Ok(new
            {
                Success = true,
                Message = "Seminar added successfully."
            });
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteSeminarData(Guid id, DeleteSeminarData deleteSeminarData)
        {
            var success = await _seminarService.DeleteData(deleteSeminarData);

            return Ok(new
            {
                Success = true,
                Message = "Seminar deleted successfully."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeminars()
        {
            var seminars = await _seminarService.GetSeminarsAsync();
            return Ok(seminars);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateData(Guid id, UpdateSeminarData updatedSeminar)
        {
            var response = await _seminarService.UpdateSeminar(updatedSeminar);
            return Ok(new
            {
                Success = true,
                Message = "Seminar updated successfully."
            });
        }
    }
}
