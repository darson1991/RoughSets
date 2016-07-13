﻿using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Algorithms;
using BusinessLogic.Algorithms.Bees;
using BusinessLogic.Algorithms.CheckAllSolutions;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Algorithms.Genetic;
using BusinessLogic.Algorithms.Tabu;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;

namespace ViewModels
{
    public class SearchingRoughSetViewModel : ViewModelBase
    {
        private RelayCommand _calculateCommand;
        private BaseAlgorithm _algorithm;
        private KindOfAlgorithm _selectedAlgorithm;
        private bool _isBusy;
        private double _mutationPossibility;
        private double _crossingOverPossibility;
        private int _individualLength;
        private int _iterationWithoutImprovement;
        private int _populationSize;
        private int _tournamentSize;
        private int _tabuListLength;
        private int _numberOfEliteSolutions;
        private int _numberOfBestSolutions;
        private int _eliteNeighborhoodSize;
        private int _bestNeighborhoodSize;

        public Action GoToResultsPageAction;
        private double _gamma;

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
            private set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public double Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                Reduct.Gamma = value;
                RaisePropertyChanged(() => Gamma);
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

        public int TabuListLength
        {
            get { return _tabuListLength; }
            set
            {
                _tabuListLength = value;
                RaisePropertyChanged(() => TabuListLength);
            }
        }

        public int NumberOfEliteSolutions
        {
            get { return _numberOfEliteSolutions; }
            set
            {
                _numberOfEliteSolutions = value; 
                RaisePropertyChanged();
            }
        }

        public int NumberOfBestSolutions
        {
            get { return _numberOfBestSolutions; }
            set
            {
                _numberOfBestSolutions = value; 
                RaisePropertyChanged();
            }
        }

        public int EliteNeighborhoodSize
        {
            get { return _eliteNeighborhoodSize; }
            set
            {
                _eliteNeighborhoodSize = value; 
                RaisePropertyChanged();
           }
        }

        public int BestNeighborhoodSize 
        {
            get { return _bestNeighborhoodSize; }
            set
            {
                _bestNeighborhoodSize = value; 
                RaisePropertyChanged();
           }
        }

        public bool IsGenetic => SelectedAlgorithm == KindOfAlgorithm.Genetic;
        public bool IsTabu => SelectedAlgorithm == KindOfAlgorithm.TabuSearch;
        public bool IsBees => SelectedAlgorithm == KindOfAlgorithm.BeesColony;

        public SearchingRoughSetViewModel()
        {
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
            Messenger.Default.Send(new BestAndAllAttributesSolutionsMessage
            {
                BestSolution = _algorithm.BestSolution,
                AllAttributesSolution = _algorithm.AllAttributesSolution,
                IterationResults = _algorithm.IterationResults
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

            if (IsGenetic)
                InitializeGeneticInputValues();
            if (IsTabu)
                InitializeTabuSearchInputValues();
            if (IsBees)
                InitializeBeesColonyInputValues();
        }

        private void InitializeGeneticInputValues()
        {
            InitializeIterationWithoutImprovementValue();
            PopulationSize = 10;
            MutationPossibility = 0.1;
            CrossingOverPossibility = 0.5;
            TournamentSize = 5;
        }

        private void InitializeTabuSearchInputValues()
        {
            InitializeIterationWithoutImprovementValue();
            TabuListLength = 3;
        }

        private void InitializeBeesColonyInputValues()
        {
            InitializeIterationWithoutImprovementValue();
            PopulationSize = 10;
            NumberOfEliteSolutions = 5;
            NumberOfBestSolutions = 3;
            EliteNeighborhoodSize = 5;
            BestNeighborhoodSize = 3;
        }

        private void InitializeIterationWithoutImprovementValue()
        {
            IterationWithoutImprovement = 30;
        }

        private void InitializeAlgorithm()
        {
            BaseAlgorithmInputValues inputValues;
            switch (SelectedAlgorithm)
            {
                case KindOfAlgorithm.CheckAllSolution:
                    _algorithm = new CheckAllSolutionsAlgorithm(_individualLength, ClusteredDataObjects);
                    break;
                case KindOfAlgorithm.Genetic:
                    inputValues = PrepareGeneticInputValues();
                    _algorithm = new GeneticAlgorithm(_individualLength, ClusteredDataObjects, inputValues);
                    break;
                case KindOfAlgorithm.TabuSearch:
                    inputValues = PrepareTabuSearchInputValues();
                    _algorithm = new TabuSearchAlgorithm(_individualLength, ClusteredDataObjects, inputValues);
                    break;
                case KindOfAlgorithm.BeesColony:
                    inputValues = PrepareBeesColonyInputValues();
                    _algorithm = new BeesColonyAlgorithm(_individualLength, ClusteredDataObjects, inputValues);
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

        private TabuSearchAlgorithmInputValues PrepareTabuSearchInputValues()
        {
            return new TabuSearchAlgorithmInputValues
            {
                IterationWithoutImprovement = IterationWithoutImprovement,
                TabuListLength = TabuListLength
            };
        }

        private BaseAlgorithmInputValues PrepareBeesColonyInputValues()
        {
            return new BeesColonyAlgorithmInputValues
            {
                IterationWithoutImprovement = IterationWithoutImprovement,
                PopulationSize = PopulationSize,
                NumberOfEliteSolutions = NumberOfEliteSolutions,
                NumberOfBestSolutions = NumberOfBestSolutions,
                EliteNeighborhoodSize = EliteNeighborhoodSize,
                BestNeighborhoodSize = BestNeighborhoodSize
            };
        }
    }
}
