using System;

namespace BusinessLogic.Exceptions
{
    public class FillDataObjectsListException: Exception
    {
        public FillDataObjectsListException(Exception exception): base("There was problem with fill DataObjectsList.", exception)
        {
        }
    }
}
