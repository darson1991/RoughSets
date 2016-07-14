namespace BusinessLogic.Algorithms.Bees
{
    public class BeesColonyAlgorithmInputValues : BaseAlgorithmInputValues
    {
        public int PopulationSize { get; set; }
        public int NumberOfEliteSolutions { get; set; }
        public int NumberOfBestSolutions { get; set; }
        public int EliteNeighborhoodSize { get; set; }
        public int BestNeighborhoodSize { get; set; }
        public int EliteNeighborhoodSteps { get; set; }
        public int BestNeighborhoodSteps { get; set; }
    }
}
