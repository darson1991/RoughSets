using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Algorithms;
using BusinessLogic.Algorithms.CheckAllSolutions;
using BusinessLogic.Exceptions;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;
using ViewModels.Providers;

namespace ViewModels
{
    public class SearchingRoughSetViewModel
    {
        private BaseAlgorithm _algorithm;
        private readonly IMessageBoxProvider _messageBoxProvider;
        private int _individualLength;

        public List<ClusteredDataObject> ClusteredDataObjects { get; private set; }
        public KindOfAlgorithm SelectedAlgorithm { get; private set; }

        public SearchingRoughSetViewModel(IMessageBoxProvider messageBoxProvider)
        {
            _messageBoxProvider = messageBoxProvider;

            Messenger.Default.Register<ClusteredDataObjectsMessage>(this, SetClusteredDataObjects);
            Messenger.Default.Register<IndividualLengthMessage>(this, SetIndividualLength);
            Messenger.Default.Register<SelectedAlgorithmMessage>(this, SetSelectedAlgorithm);
        }

        private void SetIndividualLength(IndividualLengthMessage obj)
        {
            _individualLength = obj.Length;
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
