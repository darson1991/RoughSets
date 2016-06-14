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
        private KindOfAlgorithm _selectedAlgorithm;
        private bool _isBusy;

        private int _iterationWithoutImprovement;
        private int _populationSize;
        private double _mutationPossibility;
        private double _crossingOverPossibility;
        private int _tournamentSize;

        public Action GoToResultsPageAction;
        public List<ClusteredDataObject> ClusteredDataObjects { get; private set; }

        public KindOfAlgorithm SelectedAlgorithm
        {
            get { return _selectedAlgorithm; }
            private set
            {
                _selectedAlgorithm = value;
                RaisePropertyChanged(() => SelectedAlgorithm);
            }
        }

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

        public bool IsGenetic => SelectedAlgorithm == KindOfAlgorithm.Genetic;

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

            if(IsGenetic)
                InitializeGeneticInputValues();
        }

        private void InitializeGeneticInputValues()
        {
            IterationWithoutImprovement = 20;
            PopulationSize = 10;
            MutationPossibility = 0.1;
            CrossingOverPossibility = 0.5;
            TournamentSize = 5;
        }

        private void InitializeAlgorithm()
        {
            switch (SelectedAlgorithm)
            {
                case KindOfAlgorithm.CheckAllSolution:
                    _algorithm = new CheckAllSolutionsAlgorithm(_individualLength, ClusteredDataObjects);
                    break;
                case KindOfAlgorithm.Genetic:
                    var inputValues = PrepareGeneticInputValues();
                    _algorithm = new GeneticAlgorithm(_individualLength, ClusteredDataObjects, inputValues);
                    break;
                case KindOfAlgorithm.TabuSearch:
                    break;
                case KindOfAlgorithm.BeesColony:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private GeneticAlgorithmInputValues PrepareGeneticInputValues()
        {
            return new GeneticAlgorithmInputValues
            {
                IterationWithoutImprovement = IterationWithoutImprovement,
                PopulationSize = PopulationSize,
                MutationPossibility = MutationPossibility,
                CrossingOverPossibility = CrossingOverPossibility,
                TournamentSize = TournamentSize
            };
        }
    }
}
