using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Exceptions
{
    public class ExternalDataSourceException : Exception
    {
        public ExternalDataSourceException()
        {
        }

        public ExternalDataSourceException(string message)
            : base(message)
        {
        }

        public ExternalDataSourceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
