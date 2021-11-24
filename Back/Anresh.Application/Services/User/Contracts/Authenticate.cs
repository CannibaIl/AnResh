using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Application.Services.User.Contracts
{
    public class Authenticate
    {
        public class Request
        {
            public string UserName { get; set; }
            public string Password {  get; set; }
        }
        public class Response
        {
            public string Token { get; set; }
        }
    }
}
