using System.Collections.Generic;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms
{
    public abstract class BaseAlgorithm
    {
        protected readonly List<ClusteredDataObject> ClusteredDataObjects;
        protected readonly int IndividualLength;

        public Reduct BestSolution { get; set; }
        public Reduct AllAttributesSolution { get; set; }

        protected BaseAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects)
        {
            IndividualLength = individualLength;
            ClusteredDataObjects = clusteredDataObjects;
        }

        public abstract void Calculate();

        protected void CalculateApproximationForAllAttributes()
        {
            var individual = BinaryStringHelper.GenerateIndividualWithAllAttributes(IndividualLength);
            AllAttributesSolution = new Reduct(individual, ClusteredDataObjects);
        }
    }
}
