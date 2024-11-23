using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Common.Extensions
{
    public class DuplicateItemException : CustomException
    {
        public DuplicateItemException() { }
        public DuplicateItemException(string message) : base(message) { }
        public DuplicateItemException(string message, Exception innerException) : base(message, innerException) { }
    }
}
