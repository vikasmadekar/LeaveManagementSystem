using AutoMapper;
using iText.Signatures.Validation.V1.Report;
using LeaveManagementSystem.Migrations;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace LeaveManagementSystem.Repository.Implementation
{
    public class EmployeRepository : IEmployeRepository
    {
        private readonly LDbContext _context;

        public EmployeRepository(LDbContext context)
        {
            _context = context;

        }

        public async Task<Employe> RegisterAsync(Employe employe)
        {


            await _context.Employe.AddAsync(employe);
            await _context.SaveChangesAsync();
            return employe;
        }

        public async Task<Employe> LoginAsync(string email, int password)
        {
            return await _context.Employe
                .FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
        }

        public async Task<IEnumerable<Employe>> GetAllAsync()
        {
            return await _context.Employe.ToListAsync();
        }

        public async Task<Employe> GetByIdAsync(int id)
        {
            return await _context.Employe.FindAsync(id);
        }


        public async Task<Employe> UpdateAsync(Employe employe)
        {
            _context.Employe.Update(employe);
            await _context.SaveChangesAsync();
            return employe;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var employe = await _context.Employe.FindAsync(id);
            if (employe == null)
            {
                return false; // Employee not found
            }

            _context.Employe.Remove(employe);
            await _context.SaveChangesAsync();
            return true; // Successfully deleted
        }



        public async Task<LeavBalanc> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.LeavBalanc.FirstOrDefaultAsync(lb => lb.EmployeId == employeeId);
        }
        public async Task<IEnumerable<LeavRequestess>> GetAllLeaveRequestsAsync()
        {
            return await _context.LeavRequestess.ToListAsync();
        }


        public async Task<LeavRequestess> ApplyLeaveAsync(LeavRequestess leaveRequest)
        {
            await _context.LeavRequestess.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();
            return leaveRequest;
        }
        public async Task<LeavBalanc> CreateLeaveBalanceAsync(LeavBalanc leaveBalance)
        {
            await _context.LeavBalanc.AddAsync(leaveBalance);
            await _context.SaveChangesAsync();
            return leaveBalance;
        }
        public async Task UpdateLeaveBalanceAsync(LeavBalanc leaveBalance)
        {
            _context.LeavBalanc.Update(leaveBalance);
            await _context.SaveChangesAsync();
        }
        public async Task<List<LeavRequestess>> GetPendingLeaveRequestsAsync()
        {
            return await _context.LeavRequestess
                .Where(lr => lr.Status == "Pending")
                .ToListAsync();
        }

        public async Task<LeavRequestess> GetLeaveRequestByIdAsync(int requestId)
        {
            return await _context.LeavRequestess.FindAsync(requestId);
        }

        public async Task UpdateLeaveRequestAsync(LeavRequestess request)
        {
            _context.LeavRequestess.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateLeaveRRequestAsync(LeavRequestess leaveRequest)
        {
            _context.LeavRequestess.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }
        public async Task<LeavBalanc> GetLeaveBalanceByEmployeeIdAsync(int employeeId)
        {
            return await _context.LeavBalanc
                                 .FirstOrDefaultAsync(lb => lb.EmployeId == employeeId);
        }
        //public async Task<Employe> GetByEmailAsync(string email)
        //{
        //    await _context.Employe.FirstOrDefaultAsync(e => e.Email == email);

        //}
        public async Task<Employe> GetByEmailAsync(string email) =>
      await _context.Employe.FirstOrDefaultAsync(e => e.Email == email);


        ////////////////////////////////////
        ///


        //public List<ReportItem> GetReportItems()
        //{
        //    return new List<ReportItem>
        //{
        //    new ReportItem { Name = "John Doe", Status = "Present" },
        //    new ReportItem { Name = "Jane Smith", Status = "Absent" },
        //    new ReportItem { Name = "Bob Johnson", Status = "On Leave" }
        //};


        //}

        // ✔ Correct Implementation
        public async Task<Employe> GetEmployeeByIdAsync(int id)
        {
            var employee = _context.Employe.FirstOrDefault(e => e.EmployeId == id);
            return await Task.FromResult(employee);
        }

    }
}
 


