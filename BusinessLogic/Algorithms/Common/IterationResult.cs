namespace BusinessLogic.Algorithms.Common
{
    public class IterationResult
    {
        public int Iteration { get; set; }
        public double Fitness { get; set; }
        public double Approximation { get; set; }
        public int NumberOfAttributes { get; set; }

        public IterationResult(int iteration, double fitness, double approximation, int numberOfAttributes)
        {
            Iteration = iteration;
            Fitness = fitness;
            Approximation = approximation;
            NumberOfAttributes = numberOfAttributes;
        }
    }
}
