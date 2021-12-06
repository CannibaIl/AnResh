using System.Linq;

namespace Anresh.Api.Controllers.Requests
{
    public class CustomValidators
    {
        public static bool Password(string password)
        {
            var charArray = password.ToCharArray();

            return charArray.Any(ch => char.IsLetter(ch))
                && charArray.Any(ch => char.IsUpper(ch))
                && charArray.Any(ch => char.IsNumber(ch))
                && charArray.Any(ch => char.IsSymbol(ch));
        }
    }
}
