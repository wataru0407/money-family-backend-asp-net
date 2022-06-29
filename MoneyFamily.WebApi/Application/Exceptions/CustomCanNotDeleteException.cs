using System;

namespace MoneyFamily.WebApi.Application.Exceptions
{
    public class CustomCanNotDeleteException : Exception
    {
        public CustomCanNotDeleteException() : base()
        {
        }

        public CustomCanNotDeleteException(string message) : base(message)
        {
        }
        public CustomCanNotDeleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
