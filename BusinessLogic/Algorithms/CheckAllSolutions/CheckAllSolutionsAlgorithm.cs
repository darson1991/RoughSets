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
            _numberOfSolutions = (long)Math.Pow(2, _individualLength);
        }

        public override void Calculate()
        {
            for (var i = 1; i < _numberOfSolutions; i++)
            {
                var individual = BinaryStringHelper.ConvertIntToBinaryString(i, _individualLength);
                var reduct = new Reduct(individual, _clusteredDataObjects);
                if (IsBetterThenBestSolution(reduct))
                {
                    BestSolution = reduct;
                }
            }
        }

        private bool IsBetterThenBestSolution(Reduct reduct)
        {
            return BestSolution == null || BestSolution.Approximation < reduct.Approximation
                   ||
                   (BestSolution.Approximation == reduct.Approximation &&
                    BestSolution.Subset.Count > reduct.Subset.Count);
        }
    }
}
