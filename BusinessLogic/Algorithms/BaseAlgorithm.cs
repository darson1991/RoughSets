using System.Collections.Generic;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms
{
    public abstract class BaseAlgorithm
    {
        protected readonly List<ClusteredDataObject> _clusteredDataObjects;
        protected readonly int _individualLength;

        public Reduct BestSolution { get; set; }
        public Reduct AllAttributesSolution { get; set; }

        protected BaseAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects)
        {
            _individualLength = individualLength;
            _clusteredDataObjects = clusteredDataObjects;
        }

        public abstract void Calculate();

        protected void CalculateApproximationForAllAttributes()
        {
            var individual = BinaryStringHelper.GenerateIndividualWithAllAttributes(_individualLength);
            AllAttributesSolution = new Reduct(individual, _clusteredDataObjects);
        }
    }
}
