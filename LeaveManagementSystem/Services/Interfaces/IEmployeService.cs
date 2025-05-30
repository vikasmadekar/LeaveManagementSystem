﻿using LeaveManagementSystem.Models;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Services
{
    public interface IEmployeService
    {
        Task<Employe> RegisterAsync(EmployeDTO employeDTO);
        Task<Employe> LoginAsync(EmployeLogin employeLogin);
        Task<IEnumerable<Employe>> GetAllEmployeesAsync();
        Task<Employe> GetByIdAsync(int id);
        Task<EmployeDTO> UpdateAsync(int id, EmployeDTO employeDTO);
        Task<bool> DeleteAsync(int id);
        Task<LeavBalanc> GetLeaveBalancesAsync(int id);
        Task<IEnumerable<LeavRequestess>> GetLeaveRequestsAsync();
        Task<LeavRequestess> ApplyLeaveAsync(LeavDTO leaveDTO);
        Task<List<LeavRequestess>> GetPendingLeaveRequestsAsync();
        Task<LeavRequestess> ApproveLeaveRequestAsync(int requestId);
        Task<LeavRequestess> RejectLeaveRequestAsync(int requestId);
        Task<List<LeavDTO>> GetAllLeaveHistoryAsync();
        string GetToken(Employe employe);




        //////////////
        ///
        Task<string> LoginAsync(string email, int password);
        Task<string> RefreshTokenAsync(string email, string refreshToken);

      

        ////

        //Task<Employe> GetEmployeeByIdPDF(int Id);

        Task<Employe> GetEmployeeByIdAsync(int id);
        //////////////////
        ///

        Task<byte[]> GenerateEmployeeQrCodeAsync(int id);


        
    }
}





