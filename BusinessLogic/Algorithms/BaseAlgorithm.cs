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

        public Reduct BestSolution { get; set; }
        public Reduct AllAttributesSolution { get; set; }

        protected BaseAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects)
        {
            IndividualLength = individualLength;
            ClusteredDataObjects = clusteredDataObjects;
            CheckedReducts = new List<Reduct>();
        }

        public abstract void Calculate();

        protected void CalculateApproximationForAllAttributes()
        {
            var individual = BinaryStringHelper.GenerateIndividualWithAllAttributes(IndividualLength);
            AllAttributesSolution = new Reduct(individual, ClusteredDataObjects);
        }

        protected void TryAddReductToCheckedReductsList(string individual)
        {
            if (CheckedReducts.Any(r => r.Individual == individual))
                return;

            var reduct = new Reduct(individual, ClusteredDataObjects);
            CheckedReducts.Add(reduct);
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        protected bool ShouldChangeBestSolution(Reduct reduct)
        {
            return BestSolution == null || reduct.Approximation > BestSolution.Approximation
                   ||
                   (reduct.Approximation == BestSolution.Approximation &&
                    reduct.Subset.Count < BestSolution.Subset.Count);
        }
    }
}
