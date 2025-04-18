using LeaveManagementSystem.PDFHelper;
using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // ✅ Correct namespace


namespace LeaveManagementSystem.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeController : ControllerBase
    {
        private readonly IEmployeService _service;

        public EmployeController(IEmployeService service)
        {
            _service = service;
        }
        [HttpGet("leave-balance/{id}")]
        public async Task<ActionResult<LeavBalanc>> GetLeaveBalance(int id)
        {
            var leaveBalance = await _service.GetLeaveBalancesAsync(id);
            if (leaveBalance == null)
                return NotFound(new { message = "Leave balance not found." });

            return Ok(leaveBalance);
        }

        // POST /api/employee/apply-leave
        [HttpPost("apply-leave")]
        public async Task<ActionResult> ApplyLeave([FromBody] LeavDTO leaveDTO)
        {
            if (leaveDTO == null)
                return BadRequest("Leave request is null");

            try
            {
                var leaveRequest = await _service.ApplyLeaveAsync(leaveDTO);
                return Ok(new { message = "Leave request submitted successfully!", data = leaveRequest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // GET /api/employee/leave-requests
        [HttpGet("leave-requests")]
        public async Task<ActionResult<IEnumerable<LeavRequestess>>> GetLeaveRequests()
        {
            var leaveRequests = await _service.GetLeaveRequestsAsync();
            return Ok(leaveRequests);
        }

        [HttpGet("generate/{id}")]

        
        public async Task<IActionResult> GenerateEmployeePdf(int id)
        {
            var employee = await _service.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound("Employee not found.");

            var pdfBytes = PDF_Generator.GenerateEmployeePdf(employee); // ✅ Correct usage
            return File(pdfBytes, "application/pdf", $"Employee_{employee.EmployeId}.pdf");
        }
    }
}
