using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Algorithms;
using BusinessLogic.Exceptions;
using BusinessLogic.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ViewModels.Messages;
using ViewModels.Providers;

namespace ViewModels
{
    public class AlgorithmChoiceViewModel: ViewModelBase
    {
        private readonly IMessageBoxProvider _messageBoxProvider;
        private KeyValuePair<string, string> _selectedAlgorithm;
        private RelayCommand _selectAlgorithmCommand;
        private int _individualLength;

        public Action GoToSearchingRoughSetPageAction;
        public IEnumerable<KeyValuePair<string, string>> KindOfAlgorithms { get; }
        public bool CanSelectAlgorithm => SelectedAlgorithm.Key != null && SelectedAlgorithm.Value != null;
        public RelayCommand SelectAlgorithmCommand => _selectAlgorithmCommand ?? (_selectAlgorithmCommand = new RelayCommand(SelectAlgorithm));

        public KeyValuePair<string, string> SelectedAlgorithm
        {
            get { return _selectedAlgorithm; }
            set
            {
                _selectedAlgorithm = value;
                RaisePropertyChanged(() => SelectedAlgorithm);
            }
        }

        public AlgorithmChoiceViewModel(IMessageBoxProvider messageBoxProvider)
        {
            _messageBoxProvider = messageBoxProvider;

            KindOfAlgorithms = EnumHelper.GetAllValuesAndDescriptions<KindOfAlgorithm>();
            SelectedAlgorithm = KindOfAlgorithms.FirstOrDefault(e => e.Key == KindOfAlgorithm.Genetic.ToString());

            Messenger.Default.Register<IndividualLengthMessage>(this, SetIndividualLength);
        }

        private void SetIndividualLength(IndividualLengthMessage message)
        {
            _individualLength = message.Length;
        }

        private void SelectAlgorithm()
        {
            try
            {
                var kindOfAlgorithm = (KindOfAlgorithm) Enum.Parse(typeof (KindOfAlgorithm), SelectedAlgorithm.Key, true);
                if (kindOfAlgorithm == KindOfAlgorithm.CheckAllSolution && _individualLength > 20)
                    throw new IndividualToLengthToCheckAllSolutionsException();

                Messenger.Default.Send(new SelectedAlgorithmMessage
                {
                    SelectedAlgorithm = kindOfAlgorithm
                });
                GoToSearchingRoughSetPageAction?.Invoke();
            }
            catch (IndividualToLengthToCheckAllSolutionsException exception)
            {
                _messageBoxProvider.ShowMessage(exception.Message);
            }
        }
    }
}
