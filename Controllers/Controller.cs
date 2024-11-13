using BC_Api.Services;
using BC_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BC_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeminarController : ControllerBase
    {
        private readonly SeminarService _seminarService;
        private readonly ISeminar _seminar;

        // Constructor
        public SeminarController(SeminarService seminarService, ISeminar seminar)
        {
            _seminarService = seminarService;
            _seminar = seminar;
        }
     
        [HttpPost("posttobc")]
        public async Task<IActionResult> PostDataToBc(SeminarData seminarData)
        {
            var response = await _seminar.PostData(seminarData);
            return Ok(response);
        }
    }
}
