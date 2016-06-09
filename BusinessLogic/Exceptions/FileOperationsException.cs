using System;

namespace BusinessLogic.Exceptions
{
    public class FileOperationsException: Exception
    {
        public FileOperationsException(Exception exception): base("The file which you chose have bad data or haven't description file.", exception)
        {
        }
    }
}
