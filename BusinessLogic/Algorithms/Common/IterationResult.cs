namespace BusinessLogic.Algorithms.Common
{
    public class IterationResult
    {
        public int Iteration { get; set; }
        public double Fitness { get; set; }
        public int NumberOfAttributes { get; set; }

        public IterationResult(int iteration, double fitness, int numberOfAttributes)
        {
            Iteration = iteration;
            Fitness = fitness;
            NumberOfAttributes = numberOfAttributes;
        }
    }
}
