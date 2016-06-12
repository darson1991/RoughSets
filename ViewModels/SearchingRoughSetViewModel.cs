using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Algorithms;
using BusinessLogic.Algorithms.CheckAllSolutions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;
using ViewModels.Providers;

namespace ViewModels
{
    public class SearchingRoughSetViewModel : ViewModelBase
    {
        private readonly IMessageBoxProvider _messageBoxProvider;
        private RelayCommand _calculateCommand;
        private BaseAlgorithm _algorithm;
        private int _individualLength;
        private bool _isBusy;

        public Action GoToResultsPageAction;
        public List<ClusteredDataObject> ClusteredDataObjects { get; private set; }
        public KindOfAlgorithm SelectedAlgorithm { get; private set; }

        public RelayCommand CalculateCommand => _calculateCommand ?? (_calculateCommand = new RelayCommand(Calculate));

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public SearchingRoughSetViewModel(IMessageBoxProvider messageBoxProvider)
        {
            _messageBoxProvider = messageBoxProvider;

            Messenger.Default.Register<ClusteredDataObjectsMessage>(this, SetClusteredDataObjects);
            Messenger.Default.Register<IndividualLengthMessage>(this, SetIndividualLength);
            Messenger.Default.Register<SelectedAlgorithmMessage>(this, SetSelectedAlgorithm);
        }

        private void Calculate()
        {
            IsBusy = true;
            _algorithm.Calculate();
            SendBestSolutionMessage();
            IsBusy = false;
            GoToResultsPageAction();
        }

        private void SendBestSolutionMessage()
        {
            Messenger.Default.Send(new BestSolutionMessage
            {
                BestSolution = _algorithm.BestSolution
            });
        }

        private void SetIndividualLength(IndividualLengthMessage message)
        {
            _individualLength = message.Length;
        }

        private void SetClusteredDataObjects(ClusteredDataObjectsMessage message)
        {
            ClusteredDataObjects = message.ClusteredDataObjects;
        }

        private void SetSelectedAlgorithm(SelectedAlgorithmMessage message)
        {
            SelectedAlgorithm = message.SelectedAlgorithm;
            InitializeAlgorithm();
        }

        private void InitializeAlgorithm()
        {
            switch (SelectedAlgorithm)
            {
                case KindOfAlgorithm.CheckAllSolution:
                    _algorithm = new CheckAllSolutionsAlgorithm(_individualLength, ClusteredDataObjects);
                    break;
                case KindOfAlgorithm.Genetic:
                    break;
                case KindOfAlgorithm.TabuSearch:
                    break;
                case KindOfAlgorithm.BeesColony:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
