using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IEmployeService _service;
        public AdminController(IEmployeService service)
        {
            _service = service;

        }


        [HttpGet("pending-leaves")]
        public async Task<IActionResult> GetPendingLeaveRequests()
        {
            var pendingLeaves = await _service.GetPendingLeaveRequestsAsync();

            if (pendingLeaves == null || pendingLeaves.Count == 0)
            {
                return NotFound(new { message = "No pending leave requests found." });
            }

            return Ok(new { message = "Pending leave requests retrieved successfully.", data = pendingLeaves });
        }

       
    }
}
