using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Algorithms;
using BusinessLogic.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;

namespace ViewModels
{
    public class AlgorithmChoiceViewModel: ViewModelBase
    {
        private KeyValuePair<string, string> selectedAlgorithm;
        private RelayCommand _selectAlgorithmCommand;

        public Action GoToSearchingRoughSetPageAction;
        public IEnumerable<KeyValuePair<string, string>> KindOfAlgorithms { get; }
        public bool CanSelectAlgorithm => SelectedAlgorithm.Key != null && SelectedAlgorithm.Value != null;
        public RelayCommand SelectAlgorithmCommand => _selectAlgorithmCommand ?? (_selectAlgorithmCommand = new RelayCommand(SelectAlgorithm));

        public KeyValuePair<string, string> SelectedAlgorithm
        {
            get { return selectedAlgorithm; }
            set
            {
                selectedAlgorithm = value;
                RaisePropertyChanged(() => SelectedAlgorithm);
            }
        }

        public AlgorithmChoiceViewModel()
        {
            KindOfAlgorithms = EnumHelper.GetAllValuesAndDescriptions<KindOfAlgorithm>();
            SelectedAlgorithm = KindOfAlgorithms.FirstOrDefault(e => e.Key == KindOfAlgorithm.Genetic.ToString());
        }

        private void SelectAlgorithm()
        {
            Messenger.Default.Send(new SelectedAlgorithmMessage
            {
                SelectedAlgorithm = (KindOfAlgorithm) Enum.Parse(typeof(KindOfAlgorithm), SelectedAlgorithm.Key, true)
            });
            GoToSearchingRoughSetPageAction?.Invoke();
        }
    }
}
