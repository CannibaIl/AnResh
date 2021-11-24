using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Application.Services.User.Contracts
{
    public class SaltedHash
    {
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
