using FluentValidation;

namespace LeaveManagementSystem.Models
{
    public class EmployeDTOValidator : AbstractValidator<EmployeDTO>
    {
        public EmployeDTOValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(10).WithMessage("Name cannot exceed 10 characters.");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(15).WithMessage("Email cannot exceed 15 characters.");

            RuleFor(e => e.Password)
                .InclusiveBetween(1000000, 999999999) 
                .WithMessage("Password must be between 5 and 9 digits long."); 

            RuleFor(e => e.Role)
                .NotEmpty().WithMessage("Role is required.")

                .Must(role => role == "Employee" || role == "Admin")
                .WithMessage("Role must be either 'Employee' or 'Admin'.");

            RuleFor(e => e.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department cannot exceed 100 characters.");   

            RuleFor(e => e.Designation)
                .NotEmpty().WithMessage("Designation is required.")
                .MaximumLength(5).WithMessage("Designation cannot exceed 5 characters.");
        }
    }
}
