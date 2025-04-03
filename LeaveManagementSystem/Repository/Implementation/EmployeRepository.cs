using AutoMapper;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
      
    }
}
 


