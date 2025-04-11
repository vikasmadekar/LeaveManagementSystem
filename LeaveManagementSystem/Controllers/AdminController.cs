using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IEmployeService _service;
        
        public AdminController(IEmployeService service )
        {
            _service = service;
         
        }


        [HttpGet("pending-leaves")]
        [Authorize]
        public async Task<IActionResult> GetPendingLeaveRequests()
        {
            var pendingLeaves = await _service.GetPendingLeaveRequestsAsync();

            if (pendingLeaves == null || pendingLeaves.Count == 0)
            {
                return NotFound(new { message = "No pending leave requests found." });
            }

            return Ok(new { message = "Pending leave requests retrieved successfully.", data = pendingLeaves });
        }
        [HttpPut("approve-leave/{id}")]
        [Authorize]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            try
            {
                var result = await _service.ApproveLeaveRequestAsync(id);
                return Ok(new { message = "Leave approved and balance updated", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("reject-leave/{id}")]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var leaveRequest = await _service.RejectLeaveRequestAsync(id);

            if (leaveRequest == null)
            {
                return NotFound(new { message = "Leave request not found or already processed." });
            }

            return Ok(new { message = "Leave request rejected successfully.", data = leaveRequest });
        }
        [HttpGet("leave-history")]
        [Authorize]
        public async Task<IActionResult> GetAllLeaveHistory()
        {
            try
            {
                var leaveHistory = await _service.GetAllLeaveHistoryAsync();
                return Ok(leaveHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}

