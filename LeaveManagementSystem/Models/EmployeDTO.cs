namespace LeaveManagementSystem.Models
{
    public class EmployeDTO
    {
        public string Name { get; set; }


        public string Email { get; set; }


        public int Password { get; set; } // Store hashed password


        public string Role { get; set; } // "Employee" or "Admin"


        public string Department { get; set; }


        public string Designation {  get; set; }

    }
}
