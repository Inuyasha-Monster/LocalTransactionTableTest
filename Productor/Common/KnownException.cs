using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Common
{
    public class KnownException : Exception
    {
        public KnownException(string message) : base(message)
        {

        }

        public KnownException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
