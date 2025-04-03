using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Models
{
    public class LDbContext:DbContext
    {
        public LDbContext(DbContextOptions<LDbContext> options ):base (options) 
        {
             
        }
        public DbSet<Employe> Employe { get; set; }
        public DbSet<LeavBalanc> LeavBalanc { get; set; }
        public DbSet<LeavRequestess> LeavRequestess { get; set; }




    }
}
