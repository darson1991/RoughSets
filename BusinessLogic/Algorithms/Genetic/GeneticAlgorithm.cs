namespace BusinessLogic.Algorithms.Genetic
{
    public class GeneticAlgorithm: BaseAlgorithm
    {
        public double MutationPossibility { get; set; }
        public double CrossingOverPossibility { get; set; }
        public int TurnamentSize { get; set; }
        public int PopulationSize { get; set; }
        public int IterationWithoutImprovment { get; set; }

        public GeneticAlgorithm(GeneticAlgorithmInputValues inputValues)
        {
            SetInitValues();
        }

        private void SetInitValues()
        {
            MutationPossibility = 0.01;
            CrossingOverPossibility = 0.5;
            TurnamentSize = 5;
            PopulationSize = 10;
            IterationWithoutImprovment = 300;
        }

        public override void Calculate()
        {
            throw new System.NotImplementedException();
        }
    }
}
