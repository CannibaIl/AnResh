﻿namespace Anresh.Application.Services.User.Contracts
{
    public class Create
    {
        public class Request
        {
            public int EmployeeId {  get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }
        public class Response
        {
            public int Id {  get; set; }
            public int EmployeeId { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}
