using System;

namespace BusinessLogic.Exceptions
{
    public class ConvertStringToNumberException: Exception
    {
        public Exception Exception { get; private set; }

        public ConvertStringToNumberException(Exception exception): base("There was a problem with converting strings to numbers.")
        {
            Exception = exception;
        }
    }
}
