using System.Collections.Generic;
using BusinessLogic.Algorithms.Common;

namespace BusinessLogic.Algorithms
{
    public abstract class BaseAlgorithm
    {
        protected readonly List<ClusteredDataObject> _clusteredDataObjects;
        protected readonly int _individualLength;

        public Reduct BestSolution { get; set; }

        protected BaseAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects)
        {
            _individualLength = individualLength;
            _clusteredDataObjects = clusteredDataObjects;
        }

        public abstract void Calculate();
    }
}
