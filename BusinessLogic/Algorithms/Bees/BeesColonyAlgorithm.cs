using System.Collections.Generic;

namespace BusinessLogic.Algorithms.Bees
{
    public class BeesColonyAlgorithm : BaseAlgorithm
    {
        private readonly BeesColonyAlgorithmInputValues _inputValues;

        public BeesColonyAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects, BaseAlgorithmInputValues inputValues) 
            : base(individualLength, clusteredDataObjects)
        {
            _inputValues = (BeesColonyAlgorithmInputValues)inputValues;
        }

        public override void Calculate()
        {
            throw new System.NotImplementedException();
        }
    }
}
