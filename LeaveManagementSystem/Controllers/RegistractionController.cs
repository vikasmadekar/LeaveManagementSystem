using FluentValidation;
using FluentValidation.Results;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistractionController : ControllerBase
    {
        private readonly IEmployeService _employeService;
        private readonly IValidator<EmployeDTO> _validator;

        public RegistractionController(IEmployeService employeService, IValidator<EmployeDTO> validator)
        {
            _employeService = employeService;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeDTO employeDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(employeDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                if (employeDTO == null)
                    return BadRequest("Invalid employee data");

                var employee = await _employeService.RegisterAsync(employeDTO);
                return Ok(new { message = "Employee registered successfully", data = employee });

            }
            catch
            {
                return BadRequest("Employee Not Registered");
            }
            
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmployeLogin employeLogin)
        {
            try
            {
                var employee = await _employeService.LoginAsync(employeLogin);
                if (employee == null)
                    return Unauthorized("Invalid credentials");

                var token = ((EmployeService)_employeService).GetToken(employee);

                return Ok(new
                {
                    message = "Login successful",
                    token,
                    user = new
                    {
                        employee.EmployeId,
                        employee.Name,
                        employee.Email,
                        employee.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("GetAllEmploye")]

        public async Task<ActionResult<IEnumerable<Employe>>> GetAllAsync()
        {
            try
            {
                var employe = await _employeService.GetAllEmployeesAsync();
                return Ok(employe);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpGet("GetByIdAsync")]
 
        public async Task<ActionResult<Employe>> GetByIdAsync(int id)
        {
            try
            {

                var Employeee = await _employeService.GetByIdAsync(id);
                if (Employeee == null)
                {
                    return NotFound();
                }
                return Ok(Employeee);
            }

            catch
            {
                return BadRequest();



            }


        }
        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeDTO employeDTO)
        {
            var result = await _employeService.UpdateAsync(id, employeDTO);
            return result == null ? NotFound("Employee not found") : Ok(result);


        }
        [HttpDelete("DeleteEmploye")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Employee not found." });
            }
            return Ok(new { message = "Employee deleted successfully." });
        }

    }
    
}


