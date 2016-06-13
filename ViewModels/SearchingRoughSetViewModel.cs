using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Algorithms;
using BusinessLogic.Algorithms.CheckAllSolutions;
using BusinessLogic.Algorithms.Genetic;
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

        private int _iterationWithoutImprovement;
        private int _populationSize;
        private double _mutationPossibility;
        private double _crossingOverPossibility;
        private int _tournamentSize;


        public Action GoToResultsPageAction;
        private bool _isGenetic;
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

        public int IterationWithoutImprovement
        {
            get { return _iterationWithoutImprovement; }
            set
            {
                _iterationWithoutImprovement = value;
                RaisePropertyChanged(() => IterationWithoutImprovement);
            }
        }

        public int PopulationSize
        {
            get { return _populationSize; }
            set
            {
                _populationSize = value;
                RaisePropertyChanged(() => PopulationSize);
            }
        }

        public double MutationPossibility
        {
            get { return _mutationPossibility; }
            set
            {
                _mutationPossibility = value;
                RaisePropertyChanged(() => MutationPossibility);
            }
        }

        public double CrossingOverPossibility
        {
            get { return _crossingOverPossibility; }
            set
            {
                _crossingOverPossibility = value;
                RaisePropertyChanged(() => CrossingOverPossibility);
            }
        }

        public int TournamentSize
        {
            get { return _tournamentSize; }
            set
            {
                _tournamentSize = value;
                RaisePropertyChanged(() => TournamentSize);
            }
        }

        public bool IsGenetic
        {
            get { return _isGenetic; }
            set
            {
                _isGenetic = value; 
                RaisePropertyChanged(() => IsGenetic);
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
            InitializeAlgorithm();
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
        }

        private void InitializeAlgorithm()
        {
            switch (SelectedAlgorithm)
            {
                case KindOfAlgorithm.CheckAllSolution:
                    _algorithm = new CheckAllSolutionsAlgorithm(_individualLength, ClusteredDataObjects);
                    IsGenetic = false;
                    break;
                case KindOfAlgorithm.Genetic:
                    //_algorithm = new GeneticAlgorithm();
                    IsGenetic = true;
                    break;
                case KindOfAlgorithm.TabuSearch:
                    IsGenetic = false;
                    break;
                case KindOfAlgorithm.BeesColony:
                    IsGenetic = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
