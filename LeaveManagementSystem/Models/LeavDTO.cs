using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class LeavDTO
    {
        [Key]
        public int LeavRequestID {  get; set; }  // Primary Key

        [Required]
        [ForeignKey("Employe")]
        public int EmployeId { get; set; } // Foreign Key to Employee table


        public string LeaveType { get; set; } // Annual, Sick, Casual, etc.


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }


        public string Reason { get; set; }


        public string Status { get; set; } = "Pendding";
    }
}
