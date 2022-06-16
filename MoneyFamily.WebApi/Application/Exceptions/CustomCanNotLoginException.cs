using System;

namespace MoneyFamily.WebApi.Application.Exceptions
{
    public class CustomCanNotLoginException : Exception
    {
        public CustomCanNotLoginException() : base()
        {
        }

        public CustomCanNotLoginException(string message) : base(message)
        {
        }
        public CustomCanNotLoginException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
