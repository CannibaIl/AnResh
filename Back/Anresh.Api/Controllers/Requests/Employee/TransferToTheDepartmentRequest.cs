namespace Anresh.Api.Controllers.Requests.Employee
{
    public class TransferToTheDepartmentRequest
    {
        public int OldDepartmentId {  get; set; }
        public int NewDepartmentId {  get; set; }
    }
}
