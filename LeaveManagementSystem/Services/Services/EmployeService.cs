using LeaveManagementSystem.Models;
using LeaveManagementSystem.Repository;
using AutoMapper;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Repository.Implementation;

namespace LeaveManagementSystem.Services
{
    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeService(IEmployeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

            var leaveBalance = await _repository.GetByEmployeeIdAsync(leaveDTO.EmployeId);
            if (leaveBalance == null)
                throw new Exception("Leave balance not found!");

            switch (leaveDTO.LeaveType.ToLower())
            {
                case "annual":
                    if (leaveBalance.AnnualLeave <= 0) throw new Exception("No Annual Leave Left!");
                    leaveBalance.AnnualLeave--;
                    break;
                case "sick":
                    if (leaveBalance.SickLeave <= 0) throw new Exception("No Sick Leave Left!");
                    leaveBalance.SickLeave--;
                    break;
                case "casual":
                    if (leaveBalance.CasualLeave <= 0) throw new Exception("No Casual Leave Left!");
                    leaveBalance.CasualLeave--;
                    break;
                default:
                    throw new Exception("Invalid Leave Type!");
            }

            await _repository.UpdateLeaveBalanceAsync(leaveBalance);
            return await _repository.ApplyLeaveAsync(leaveRequest);
        }
        public async Task<List<LeavRequestess>> GetPendingLeaveRequestsAsync()
        {
            return await _repository.GetPendingLeaveRequestsAsync();
        }
        
    }
    
}


