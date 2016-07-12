using System.Collections.Generic;
using BusinessLogic.Algorithms.Common;

namespace ViewModels.Messages
{
    public class BestAndAllAttributesSolutionsMessage
    {
        public Reduct BestSolution { get; set; }
        public Reduct AllAttributesSolution { get; set; }
        public List<IterationResult> IterationResults { get; set; } 
    }
}
