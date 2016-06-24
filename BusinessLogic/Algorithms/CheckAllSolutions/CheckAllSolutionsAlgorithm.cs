using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms.CheckAllSolutions
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class CheckAllSolutionsAlgorithm : BaseAlgorithm
    {
        private readonly long _numberOfSolutions;

        public CheckAllSolutionsAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects)
            : base(individualLength, clusteredDataObjects)
        {
            _numberOfSolutions = (long)Math.Pow(2, IndividualLength);
        }

        public override void Calculate()
        {
            CalculateApproximationForAllAttributes();
            for (var i = 1; i < _numberOfSolutions; i++)
            {
                var individual = BinaryStringHelper.ConvertIntToBinaryString(i, IndividualLength);
                var reduct = new Reduct(individual, ClusteredDataObjects);
                if (ShouldChangeBestSolution(reduct))
                    BestSolution = reduct;
            }
        }
    }
}
