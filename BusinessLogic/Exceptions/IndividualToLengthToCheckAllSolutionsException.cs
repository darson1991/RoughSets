using System;

namespace BusinessLogic.Exceptions
{
    public class IndividualToLengthToCheckAllSolutionsException : Exception
    {
        public IndividualToLengthToCheckAllSolutionsException() : base("Individual is to long to check all solutions.")
        {
        }
    }
}
