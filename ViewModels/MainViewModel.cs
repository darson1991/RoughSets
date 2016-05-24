using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic;
using BusinessLogic.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ViewModels.Providers;

namespace ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IOpenFileDialogProvider _openFileDialogProvider;
        private readonly IMessageBoxProvider _messageBoxProvider;

        private RelayCommand _browseFileCommand;
        private RelayCommand _fillDataCommand;
        private string _contentFileUrl;
        private string _roughSetsFileContent;
        private string _attributesDescription;

        public RelayCommand BrowseFileCommand => _browseFileCommand ?? (_browseFileCommand = new RelayCommand(BrowseFile));
        public RelayCommand FillDataCommand => _fillDataCommand ?? (_fillDataCommand = new RelayCommand(FillData));

        public string ContentFileUrl
        {
            get { return _contentFileUrl; }
            set
            {
                _contentFileUrl = value;
                RaisePropertyChanged();
            }
        }

        public string DescritpionFileUrl => ContentFileUrl.Insert(ContentFileUrl.Length - 4, "_descr");

        public RoughSetInformations RoughSetInformations { get; private set; }

        public MainViewModel(IOpenFileDialogProvider openFileDialogProvider, IMessageBoxProvider messageBoxProvider)
        {
            _openFileDialogProvider = openFileDialogProvider;
            _messageBoxProvider = messageBoxProvider;
        }

        private void BrowseFile()
        {
            _openFileDialogProvider.ExecuteOpenFileDialog();
            ContentFileUrl = _openFileDialogProvider.SelectedPath;
        }

        private void FillData()
        {
            try
            {
                ReadContentAndDescriptionFiles();
                PrepareRoughSetInformations();
                PrepareDataObjects();
            }
            catch (Exception)
            {
                _messageBoxProvider.ShowMessage("The file which you chose have bad data or haven't description file.");
            }
        }

        private void ReadContentAndDescriptionFiles()
        {
            _roughSetsFileContent = FileOperations.GetFileContent(ContentFileUrl);
            _attributesDescription = FileOperations.GetFileContent(DescritpionFileUrl);
        }

        private void PrepareRoughSetInformations()
        {
            var stringSeparators = new[] { "\r\n" };
            var lines = _attributesDescription.Substring(0, _attributesDescription.Length - 2).Split(stringSeparators, StringSplitOptions.None);
            var argumentNames = PrepareArgumentNames(lines);
            var decisionClasses = PrepareDecisionClasses(lines);

            CreateRoughSetInformations(argumentNames, decisionClasses);
        }

        private void PrepareDataObjects()
        {
            var stringSeparators = new[] { "\r\n" };
            var lines = _roughSetsFileContent.Substring(0, _roughSetsFileContent.Length - 2).Split(stringSeparators, StringSplitOptions.None);
            var convertDataToNumbers = new ConvertDataToNumbers();
            convertDataToNumbers.PrepareListOfStringColumns(lines[0]);
        }

        private static List<string> PrepareArgumentNames(IReadOnlyList<string> lines)
        {
            if(lines == null || lines.Count == 0)
                return new List<string>();

            var argumentNames = lines[0].Split(',').ToList();
            return argumentNames;
        }

        private static List<string> PrepareDecisionClasses(IReadOnlyList<string> lines)
        {
            var decisionClasses = new List<string>();

            for (var i = 1; i < lines.Count; i++)
            {
                decisionClasses.Add(lines[i].Split(':')[0]);
            }
            return decisionClasses;
        }

        private void CreateRoughSetInformations(List<string> argumentNames, List<string> decisionClasses)
        {
            RoughSetInformations = new RoughSetInformations(argumentNames, decisionClasses);
        }
    }
}
