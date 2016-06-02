using System.Collections.Generic;
using BusinessLogic;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;

namespace ViewModels
{
    public class SearchingRoughSetViewModel
    {
        public List<ClusteredDataObject> ClusteredDataObjects { get; private set; }

        public SearchingRoughSetViewModel()
        {
            Messenger.Default.Register<ClusteredDataObjectsMessage>(this, SetClusteredDataObjects);
        }

        private void SetClusteredDataObjects(ClusteredDataObjectsMessage message)
        {
            ClusteredDataObjects = message.ClusteredDataObjects;
        }
    }
}
