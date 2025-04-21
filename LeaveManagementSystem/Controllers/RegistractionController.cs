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
        public async Task<IActionResult> Login(EmployeLogin dto)
        {
            var token = await _employeService.LoginAsync(dto.Email, dto.Password);
            return token == null ? Unauthorized("Invalid login") : Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(EmployeLogin dto)
        {
            var token = await  _employeService.RefreshTokenAsync(dto.Email, dto.Password.ToString());
            return token == null ? Unauthorized("Invalid refresh token") : Ok(token);
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

        [HttpGet("qrcode/{id}")]
        public async Task<IActionResult> GetEmployeeQrCode(int id)
        {
            var qrCodeBytes = await _employeService.GenerateEmployeeQrCodeAsync(id);
            if (qrCodeBytes == null)
                return NotFound("Employee not found");

            return File(qrCodeBytes, "image/png");
        }


    }

}


