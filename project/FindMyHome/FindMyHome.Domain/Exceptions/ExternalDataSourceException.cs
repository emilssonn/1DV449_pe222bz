using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Exceptions
{
	/// <summary>
	/// If a API call returns a 5xx error this exception is thrown
	/// </summary>
    public class ExternalDataSourceException : Exception
    {
		public string DetailedMessage { get; set; }

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

		public ExternalDataSourceException(string message, string detailedMessage, Exception inner)
			: base(message, inner)
		{
			this.DetailedMessage = detailedMessage;
		}
    }
}
