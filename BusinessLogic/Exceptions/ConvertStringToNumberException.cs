using System;

namespace BusinessLogic.Exceptions
{
    public class ConvertStringToNumberException: Exception
    {
        public ConvertStringToNumberException(Exception exception): base("There was a problem with converting strings to numbers.", exception)
        {
        }
    }
}
