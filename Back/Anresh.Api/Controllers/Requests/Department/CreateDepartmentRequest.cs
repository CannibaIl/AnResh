using FluentValidation;

namespace Anresh.Api.Controllers.Requests.Department
{
    public class CreateDepartmentRequest
    {
        public string Name { get; set; }
    }
    public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentRequestValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Enter Name")
                 .MaximumLength(20).WithMessage("Max length 20");
        }
    }
}