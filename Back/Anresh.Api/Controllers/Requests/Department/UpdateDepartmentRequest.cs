using FluentValidation;

namespace Anresh.Api.Controllers.Requests.Department
{
    public class UpdateDepartmentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateDepartmentRequestValidator : AbstractValidator<UpdateDepartmentRequest>
    {
        public UpdateDepartmentRequestValidator()
        {
            RuleFor(x => x.Id)
                 .NotEmpty().WithMessage("Empty id");

            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Enter Name")
                 .MaximumLength(20).WithMessage("Max length 20");
        }
    }
}
