using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Anresh.Api.Controllers.Requests.Employee
{
    public class CreateEmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
    }

    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage("Enter FirstName")
                 .MaximumLength(20).WithMessage("Max length 20");

            RuleFor(x => x.LastName)
                 .NotEmpty().WithMessage("Enter LastName")
                 .MaximumLength(20).WithMessage("Max length 20");

            RuleFor(x => x.DepartmentId)
                 .NotEmpty().WithMessage("Select department");

            RuleFor(x => x.Salary)
                 .NotEmpty().WithMessage("Enter salary");
        }
    }
}
