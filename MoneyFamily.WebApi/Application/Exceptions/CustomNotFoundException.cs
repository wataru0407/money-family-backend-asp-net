using System;

namespace MoneyFamily.WebApi.Application.Exceptions
{
    public class CustomNotFoundException : Exception
    {
        public CustomNotFoundException() : base()
        {
        }

        public CustomNotFoundException(string message) : base(message)
        {
        }
        public CustomNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
