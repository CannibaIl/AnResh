using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Application.Services.User.Contracts
{
    public class Register
    {
        public class Request
        {
            public int EmployeeId { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class Response
        {
            public int Id { get; set; }
            public int EmployeeId { get; set; }
            public string Email {  get; set; }
            public string Role { get; set; }
            public string Token {  get; set; }
        }
    }
}
