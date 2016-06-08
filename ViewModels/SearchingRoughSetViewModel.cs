using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Algorithms;
using BusinessLogic.Algorithms.CheckAllSolutions;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;

namespace ViewModels
{
    public class SearchingRoughSetViewModel
    {
        private BaseAlgorithm _algorithm;

        public List<ClusteredDataObject> ClusteredDataObjects { get; private set; }
        public KindOfAlgorithm SelectedAlgorithm { get; private set; }

        public SearchingRoughSetViewModel()
        {
            Messenger.Default.Register<ClusteredDataObjectsMessage>(this, SetClusteredDataObjects);
            Messenger.Default.Register<SelectedAlgorithmMessage>(this, SetSelectedAlgorithm);
        }

        private void SetClusteredDataObjects(ClusteredDataObjectsMessage message)
        {
            ClusteredDataObjects = message.ClusteredDataObjects;
        }

        private void SetSelectedAlgorithm(SelectedAlgorithmMessage message)
        {
            SelectedAlgorithm = message.SelectedAlgorithm;
            InitializeAlgorithm();

            _algorithm.Calculate();
        }

        private void InitializeAlgorithm()
        {
            switch (SelectedAlgorithm)
            {
                case KindOfAlgorithm.CheckAllSolution:
                    try
                    {
                        _algorithm = new CheckAllSolutionsAlgorithm(ClusteredDataObjects[0].Arguments.Count,
                            ClusteredDataObjects);
                    }
                    catch (Exception exception)
                    {
                        //TODO: messageBox
                    }
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
