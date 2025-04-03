using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class Employe
    {
        [Key]
        public int EmployeId { get; set; }

       
        public string Name { get; set; }

       
        public string Email { get; set; }

      
        public int Password { get; set; } // Store hashed password

      
        public string Role { get; set; } // "Employee" or "Admin"

       
        public string Department { get; set; }

      
        public string Designation { get; set; }
    }
}

