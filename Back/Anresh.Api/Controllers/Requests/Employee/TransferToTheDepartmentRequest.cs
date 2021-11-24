using FluentValidation;

namespace Anresh.Api.Controllers.Requests.Employee
{
    public class TransferToTheDepartmentRequest
    {
        public int OldDepartmentId {  get; set; }
        public int NewDepartmentId {  get; set; }
    }
    public class TransferToTheDepartmentRequestValidator : AbstractValidator<TransferToTheDepartmentRequest>
    {
        public TransferToTheDepartmentRequestValidator()
        {
            RuleFor(x => x.OldDepartmentId)
                 .NotEmpty().WithMessage("Empty old department id");

            RuleFor(x => x.NewDepartmentId)
                 .NotEmpty().WithMessage("Empty new department id");


        }
    }
}
