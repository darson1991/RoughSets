using System.ComponentModel;

namespace BusinessLogic.Algorithms
{
    public enum KindOfAlgorithm
    {
        [Description("Check all solution")]
        CheckAllSolution,
        [Description("Genetic")]
        Genetic,
        [Description("Tabu search")]
        TabuSearch,
        [Description("Bees colony")]
        BeesColony
    }
}
