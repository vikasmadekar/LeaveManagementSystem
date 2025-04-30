using Azure.Core;
using LeaveManagementSystem.Migrations;
using LeaveManagementSystem.Models;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Runtime.Intrinsics.X86;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using iText.Signatures.Validation.V1.Report;

namespace LeaveManagementSystem.Repository
{
    public interface IEmployeRepository
    {
        Task<Employe> RegisterAsync(Employe employe);
        Task<Employe> LoginAsync(string email, int password);
        Task<IEnumerable<Employe>> GetAllAsync();
        Task<Employe> GetByIdAsync(int id);
        Task<Employe> UpdateAsync(Employe employe);
        Task<bool> DeleteAsync(int id);
        Task<LeavBalanc> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<LeavRequestess>> GetAllLeaveRequestsAsync();
        Task<LeavRequestess> ApplyLeaveAsync(LeavRequestess leaveRequest);
        Task<LeavBalanc> CreateLeaveBalanceAsync(LeavBalanc leaveBalance);
        Task UpdateLeaveBalanceAsync(LeavBalanc leaveBalance);
        Task<List<LeavRequestess>> GetPendingLeaveRequestsAsync();
        Task<LeavRequestess> GetLeaveRequestByIdAsync(int requestId);
       Task UpdateLeaveRequestAsync(LeavRequestess request);
        Task UpdateLeaveRRequestAsync(LeavRequestess leaveRequest);

        //------------
      
        Task<LeavBalanc> GetLeaveBalanceByEmployeeIdAsync(int employeeId); // Appproved Request

        ///////////////////
        Task<Employe> GetByEmailAsync(string email);



        //////////////////////////
        /// <summary>
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 



        Task<Employe> GetEmployeeByIdAsync(int id);

        
        Task<Employe> AddAsync(Employe employe);
        Task<Employe> GetByEmailAndPasswordAsync(string email, int password);
        
    }
}
