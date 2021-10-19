using System.ComponentModel.DataAnnotations;

namespace Anresh.Api.Controllers.Requests.Department
{
    public class CreateDepartmentRequest
    {
        [Required]
        public string Name { get; set; }
    }
}