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

    }

}


