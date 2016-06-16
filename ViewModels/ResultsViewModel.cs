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

        public ResultsViewModel()
        {
            Messenger.Default.Register<BestAndAllAttributesSolutionsMessage>(this, SetBestSolution);
        }

        private void SetBestSolution(BestAndAllAttributesSolutionsMessage message)
        {
            BestSolution = message.BestSolution;
            AllAttributesSolution = message.AllAttributesSolution;
        }
    }
}
