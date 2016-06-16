using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms.Tabu
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class TabuSearchAlgorithm: BaseAlgorithm
    {
        private readonly TabuSearchAlgorithmInputValues _inputValues;
        private int _iterationWithoutImprovementCount;

        public Reduct ActualSolution { get; set; }

        public TabuSearchAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects, BaseAlgorithmInputValues inputValues)
            : base(individualLength, clusteredDataObjects)
        {
            _inputValues = (TabuSearchAlgorithmInputValues)inputValues;
        }

        public override void Calculate()
        {
            CalculateApproximationForAllAttributes();
            SetInitialSolution();

            if (ShouldChangeBestSolution())
                BestSolution = ActualSolution;

            while (++_iterationWithoutImprovementCount != _inputValues.IterationWithoutImprovement)
            {
                //TODO: add first part of tabu search algorithm

                if (!ShouldChangeBestSolution())
                    continue;

                BestSolution = ActualSolution;
                _iterationWithoutImprovementCount = 0;
            }
        }

        private void SetInitialSolution()
        {
            var individual = BinaryStringHelper.GenerateRandomIndividual(_individualLength);
            ActualSolution = new Reduct(individual, _clusteredDataObjects);
        }

        private bool ShouldChangeBestSolution()
        {
            return BestSolution == null || ActualSolution.Approximation > BestSolution.Approximation
                   ||
                   (ActualSolution.Approximation == BestSolution.Approximation &&
                    ActualSolution.Subset.Count < BestSolution.Subset.Count);
        }
    }
}
