using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Common.Extensions
{
    public class ServerErrorException : CustomException
    {
        public ServerErrorException() { }
        public ServerErrorException(string message) : base(message) { }
        public ServerErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
