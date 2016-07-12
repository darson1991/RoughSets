using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms
{
    public abstract class BaseAlgorithm
    {
        protected readonly List<ClusteredDataObject> ClusteredDataObjects;
        protected readonly int IndividualLength;
        protected List<Reduct> CheckedReducts;
        protected int IterationWithoutImprovementCount;
        protected int IterationNumber;

        public Reduct BestSolution { get; set; }
        public Reduct AllAttributesSolution { get; set; }
        public List<IterationResult> IterationResults { get; set; } 

        protected BaseAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects)
        {
            IndividualLength = individualLength;
            ClusteredDataObjects = clusteredDataObjects;
            CheckedReducts = new List<Reduct>();
            IterationResults = new List<IterationResult>();
            IterationNumber = 0;
        }

        public abstract void Calculate();

        protected void AddToIterationResultsList(double fitness, int attributesCount)
        {
            IterationResults.Add(new IterationResult(IterationNumber, fitness, attributesCount));
        }

        protected void CalculateApproximationForAllAttributes()
        {
            var individual = BinaryStringHelper.GenerateIndividualWithAllAttributes(IndividualLength);
            AllAttributesSolution = new Reduct(individual, ClusteredDataObjects);
        }

        protected void TryAddReductToCheckedReductsList(string individual)
        {
            if (CheckedReducts.Any(r => r.Individual == individual))
                return;

            CreateNewReduct(individual);
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        protected bool ShouldChangeBestSolution(Reduct reduct)
        {
            return BestSolution == null || reduct.Approximation > BestSolution.Approximation
                   ||
                   (reduct.Approximation == BestSolution.Approximation &&
                    reduct.Subset.Count < BestSolution.Subset.Count);
        }

        protected void TryToUpdateBestSolution(Reduct reduct)
        {
            if (!ShouldChangeBestSolution(reduct))
                return;

            BestSolution = reduct;
            IterationWithoutImprovementCount = 0;
        }

        private void CreateNewReduct(string individual)
        {
            var reduct = new Reduct(individual, ClusteredDataObjects);
            CheckedReducts.Add(reduct);
        }
    }
}
