﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Domain.Exceptions
{
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

		public BadRequestException(string message, string detailedMessage, Exception inner)
			: base(message, inner)
		{
			this.DetailedMessage = detailedMessage;
		}
	}
}