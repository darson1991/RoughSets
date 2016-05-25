using System;

namespace BusinessLogic.Exceptions
{
    public class FillDataObjectsListException: Exception
    {
        public Exception Exception { get; private set; }

        public FillDataObjectsListException(Exception exception): base("There was problem with fill DataObjectsList.")
        {
            Exception = exception;
        }
    }
}
