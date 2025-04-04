using FluentValidation;

namespace LeaveManagementSystem.Models
{
    public class EmployeDTOValidator : AbstractValidator<EmployeDTO>
    {
        public EmployeDTOValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(20).WithMessage("Name cannot exceed 20 characters.");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(20).WithMessage("Email cannot exceed 20 characters.");




            RuleFor(e => e.Password)
            .InclusiveBetween(0, 99999)
           .WithMessage("Password must be up to 5 digits long.");


            RuleFor(e => e.Role)
                .NotEmpty().WithMessage("Role is required.")

                .Must(role => role == "Employee" || role == "Admin")
                .WithMessage("Role must be either 'Employee' or 'Admin'.");

            RuleFor(e => e.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department cannot exceed 100 characters.");   

            RuleFor(e => e.Designation)
                .NotEmpty().WithMessage("Designation is required.")
                .MaximumLength(20).WithMessage("Designation cannot exceed 20 characters.");
        }
    }
}
