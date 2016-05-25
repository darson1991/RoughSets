using System;
using System.Runtime.Serialization;

namespace BusinessLogic.Exceptions
{
    public class FileOperationsException: Exception
    {
        public Exception Exception { get; private set; }
            
        public FileOperationsException(Exception exception): base("The file which you chose have bad data or haven't description file.")
        {
            Exception = exception;
        }
    }
}
