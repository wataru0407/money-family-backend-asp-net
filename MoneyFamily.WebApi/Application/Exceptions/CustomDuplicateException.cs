using System;

namespace MoneyFamily.WebApi.Application.Exceptions
{
    public class CustomDuplicateException : Exception
    {
        public CustomDuplicateException() : base()
        {
        }

        public CustomDuplicateException(string message) : base(message)
        {
        }
        public CustomDuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
