using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandler
{
    public class ResourceException : ApplicationException
    {
        public ResourceException(string message) : base(message)
        {
        }
    }
}