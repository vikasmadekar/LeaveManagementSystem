using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class LeavBalanc
    {
        [Key]
        public int LeavRequestID { get; set; } // Primary Key

        [Required]
        [ForeignKey("Employe")]
        public int EmployeId { get; set; } // Foreign Key to Employee table


     
        public int AnnualLeave { get; set; }  =15;


        public int SickLeave { get; set; } = 7;


        public int CasualLeave { get; set; } = 9;

        public int? OtherLeave { get; set; } = 2;

    }
}
    