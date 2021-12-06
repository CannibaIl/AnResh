namespace Anresh.Application.Services.User.Contracts
{
    public class RestorePassword
    {
        public class Request
        {
            public string Token { get; set; }
            public string Password { get; set; }
        }
        public class Response
        {
            public string Email { get; set; }
        }
    }
}
