using BusinessLogic.Algorithms.Common;

namespace BusinessLogic.Algorithms
{
    public abstract class BaseAlgorithm
    {
        public Reduct BestSolution { get; set; }

        public abstract void Calculate();
    }
}
