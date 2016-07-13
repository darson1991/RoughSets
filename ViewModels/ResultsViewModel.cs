using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Algorithms.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;

namespace ViewModels
{
    public class ResultsViewModel: ViewModelBase
    {
        public Reduct BestSolution { get; set; }
        public Reduct AllAttributesSolution { get; set; }
        public List<IterationResult> IterationResults { get; set; }
        public int IterationsAxisMaxSize => IterationResults.Count + 1;
        public double FitnessAxisMaxSize => 1.2 * IterationResults.Max(i => i.Fitness);
        public int ReductAxisMaxSize => (int)(1.2 * IterationResults.Max(i => i.NumberOfAttributes));

        public ResultsViewModel()
        {
            Messenger.Default.Register<BestAndAllAttributesSolutionsMessage>(this, SetBestSolution);
        }

        private void SetBestSolution(BestAndAllAttributesSolutionsMessage message)
        {
            BestSolution = message.BestSolution;
            AllAttributesSolution = message.AllAttributesSolution;
            IterationResults = message.IterationResults;
        }
    }
}
