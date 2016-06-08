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
        private readonly List<ClusteredDataObject> _clusteredDataObjects;
        private readonly int _individualLength;

        public CheckAllSolutionsAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects)
        {
            _individualLength = individualLength;
            _numberOfSolutions = (long)Math.Pow(2, individualLength);
            _clusteredDataObjects = clusteredDataObjects;
        }

        public override void Calculate()
        {
            for (var i = 1; i < _numberOfSolutions; i++)
            {
                var individual = BinaryStringHelper.ConvertIntToBinaryString(i, _individualLength);
                var reduct = new Reduct(individual, _clusteredDataObjects);
                if (BestSolution == null || BestSolution.Approximation < reduct.Approximation
                    ||
                    (BestSolution.Approximation == reduct.Approximation &&
                     BestSolution.Subset.Count > reduct.Subset.Count))
                {
                    BestSolution = reduct;
                }
            }
        }
    }
}
