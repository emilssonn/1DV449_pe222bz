using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Exceptions
{
	/// <summary>
	/// If a API call returns a bad request http error or
	/// if any other values given to a method in this project is not correct
	/// this exception is thrown
	/// </summary>
	public class BadRequestException : Exception
	{
		public string DetailedMessage { get; set; }

		public BadRequestException()
		{
		}

		public BadRequestException(string message)
			: base(message)
		{
		}

		public BadRequestException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public BadRequestException(string message, string detailedMessage, Exception inner = null)
			: base(message, inner)
		{
			this.DetailedMessage = detailedMessage;
		}
	}
}
