namespace BusinessLogic.Algorithms.Genetic
{
    public class GeneticAlgorithmInputValues: BaseAlgorithmInputValues
    {
        public int PopulationSize { get; set; }
        public double MutationPossibility { get; set; }
        public double CrossingOverPossibility { get; set; }
        public int TournamentSize { get; set; }
    }
}
