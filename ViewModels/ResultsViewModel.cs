using BusinessLogic.Algorithms.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;

namespace ViewModels
{
    public class ResultsViewModel: ViewModelBase
    {
        public Reduct BestSolution { get; set; }

        public ResultsViewModel()
        {
            Messenger.Default.Register<BestSolutionMessage>(this, SetBestSolution);
        }

        private void SetBestSolution(BestSolutionMessage message)
        {
            BestSolution = message.BestSolution;
        }
    }
}
