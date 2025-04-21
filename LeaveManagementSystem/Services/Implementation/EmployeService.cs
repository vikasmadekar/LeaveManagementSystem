using LeaveManagementSystem.Models;
using LeaveManagementSystem.Repository;
using AutoMapper;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Repository.Implementation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeaveManagementSystem.Helper;
//using LeaveManagementSystem.PDF_Helper;

namespace LeaveManagementSystem.Services
{
    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenHelper _tokenHelper;

       // private readonly PDF_Generator _pDF_Generator;

        public EmployeService(IEmployeRepository repository, IMapper mapper , IConfiguration configuration, JwtTokenHelper tokenHelper)//PDF_Generator pDF_Generator)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _tokenHelper = tokenHelper;
           // _pDF_Generator = pDF_Generator;
        }

        public async Task<Employe> RegisterAsync(EmployeDTO employeDTO)
        {
            var employe = _mapper.Map<Employe>(employeDTO);
            var registeredEmploye = await _repository.RegisterAsync(employe);

            var existingLeaveBalance = await _repository.GetByEmployeeIdAsync(registeredEmploye.EmployeId);
            if (existingLeaveBalance == null)
            {
                var leaveBalance = new LeavBalanc
                {
                    EmployeId = registeredEmploye.EmployeId,
                    AnnualLeave = 15,
                    SickLeave = 7,
                    CasualLeave = 9,
                    OtherLeave = 2
                };
                await _repository.CreateLeaveBalanceAsync(leaveBalance);
            }

            return registeredEmploye;
        }

        public async Task<Employe> LoginAsync(EmployeLogin employeLogin)
        {
            return await _repository.LoginAsync(employeLogin.Email, employeLogin.Password);
        }

        public string GetToken(Employe employe)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, employe.EmployeId.ToString()),
                    new Claim(ClaimTypes.Email, employe.Email),
                    new Claim(ClaimTypes.Role, employe.Role ?? "User")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<IEnumerable<Employe>> GetAllEmployeesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Employe> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<EmployeDTO> UpdateAsync(int id, EmployeDTO employeDTO)
        {
            var existingEmploye = await _repository.GetByIdAsync(id);
            if (existingEmploye == null) return null;

            _mapper.Map(employeDTO, existingEmploye); // ✅ Map DTO to Entity

            var updatedEmploye = await _repository.UpdateAsync(existingEmploye);
            return _mapper.Map<EmployeDTO>(updatedEmploye);

        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<LeavBalanc> GetLeaveBalancesAsync(int id)
        {
            return await _repository.GetByEmployeeIdAsync(id);
        }
        public async Task<IEnumerable<LeavRequestess>> GetLeaveRequestsAsync()
        {
            return await _repository.GetAllLeaveRequestsAsync();
        }

        public async Task<LeavRequestess> ApplyLeaveAsync(LeavDTO leaveDTO)
        {
            var leaveRequest = _mapper.Map<LeavRequestess>(leaveDTO);
            leaveRequest.Status = "Pending";
            leaveRequest.DateSubmitted = DateTime.UtcNow;

            return await _repository.ApplyLeaveAsync(leaveRequest);
        }
        public async Task<List<LeavRequestess>> GetPendingLeaveRequestsAsync()
        {
            return await _repository.GetPendingLeaveRequestsAsync();
        }
        public async Task<LeavRequestess> ApproveLeaveRequestAsync(int id)
        {
            var leaveRequest = await _repository.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null || leaveRequest.Status == "Approved")
                throw new Exception("Invalid or already approved leave request.");

            var leaveBalance = await _repository.GetLeaveBalanceByEmployeeIdAsync(leaveRequest.EmployeId);

            int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

            switch (leaveRequest.LeaveType.ToLower())
            {
                case "annual":
                    if (leaveBalance.AnnualLeave < leaveDays)
                        throw new Exception("Insufficient annual leave.");
                    leaveBalance.AnnualLeave -= leaveDays;
                    break;

                case "sick":
                    if (leaveBalance.SickLeave < leaveDays)
                        throw new Exception("Insufficient sick leave.");
                    leaveBalance.SickLeave -= leaveDays;
                    break;

                case "casual":
                    if (leaveBalance.CasualLeave < leaveDays)
                        throw new Exception("Insufficient casual leave.");
                    leaveBalance.CasualLeave -= leaveDays;
                    break;

                case "other":
                    if (leaveBalance.OtherLeave == null || leaveBalance.OtherLeave < leaveDays)
                        throw new Exception("Insufficient other leave.");
                    leaveBalance.OtherLeave -= leaveDays;
                    break;

                default:
                    throw new Exception("Invalid leave type.");
            }

            leaveRequest.Status = "Approved";
            leaveRequest.AdminRemarks = "Approved by Admin";

            await _repository.UpdateLeaveBalanceAsync(leaveBalance);
            await _repository.UpdateLeaveRequestAsync(leaveRequest);

            return leaveRequest;
        }

        public async Task<LeavRequestess> RejectLeaveRequestAsync(int requestId)
        {
            var leaveRequest = await _repository.GetLeaveRequestByIdAsync(requestId);

            if (leaveRequest == null || leaveRequest.Status != "Pending")
            {
                return null; // Leave request not found or already approved/rejected
            }

            leaveRequest.Status = "Rejected";
            await _repository.UpdateLeaveRequestAsync(leaveRequest);

            return leaveRequest;
        }

        public async Task<List<LeavDTO>> GetAllLeaveHistoryAsync()
        {
            var requests = await _repository.GetAllLeaveRequestsAsync();
            return _mapper.Map<List<LeavDTO>>(requests);
        }

        //////////////////////////////////////////////////
        ///
        public async Task<string> LoginAsync(string email, int password)
        {
            var emp = await  _repository.GetByEmailAsync(email);
            if (emp == null || emp.Password != password) return null;

            emp.RefreshToken = _tokenHelper.GenerateRefreshToken();
            emp.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5);
            await  _repository.UpdateAsync(emp);

            return $"Access: {_tokenHelper.GenerateAccessToken(emp)}\nRefresh: {emp.RefreshToken}";
        }

        public async Task<string> RefreshTokenAsync(string email, string refreshToken)
        {
            var emp = await _repository.GetByEmailAsync(email);
            if (emp == null || emp.RefreshToken != refreshToken || emp.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            emp.RefreshToken = _tokenHelper.GenerateRefreshToken();
            emp.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5);
            await _repository.UpdateAsync(emp);

            return $"Access: {_tokenHelper.GenerateAccessToken(emp)}\nRefresh: {emp.RefreshToken}";
        }


        /////////////////
        public async Task<Employe> GetEmployeeByIdAsync(int id)
        {
            return await _repository.GetEmployeeByIdAsync(id);
        }
        ///////////////
        ///
        public async Task<byte[]> GenerateEmployeeQrCodeAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
                return null;

            string qrContent = $"ID: {employee.EmployeId}\nName: {employee.Name}\nEmail: {employee.Email}\nDepartment: {employee.Department}\nDesignation: {employee.Designation}";
            return QrCodeHelper.GenerateQrCode(qrContent);
        }

    }

}


