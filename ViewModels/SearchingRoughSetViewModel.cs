using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Algorithms;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;

namespace ViewModels
{
    public class SearchingRoughSetViewModel
    {
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
        }
    }
}
