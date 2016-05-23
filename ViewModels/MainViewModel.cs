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

        public MainViewModel(IOpenFileDialogProvider openFileDialogProvider)
        {
            _openFileDialogProvider = openFileDialogProvider;
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
            }
            catch (Exception exception)
            {
                
            }
        }

        private void ReadContentAndDescriptionFiles()
        {
            _roughSetsFileContent = FileOperations.GetFileContent(ContentFileUrl);
            _attributesDescription = FileOperations.GetFileContent(DescritpionFileUrl);
        }

        private void PrepareRoughSetInformations()
        {
            var lines = _attributesDescription.Substring(0, _attributesDescription.Length - 2).Split('\n');
            var argumentNames = PrepareArgumentNames(lines);
            var decisionClasses = PrepareDecisionClasses(lines);

            CreateRoughSetInformations(argumentNames, decisionClasses);
        }

        private static List<string> PrepareArgumentNames(IReadOnlyList<string> lines)
        {
            if(lines == null || lines.Count == 0)
                return new List<string>();
            var argumentNames = lines[0].Split(',').ToList();
            RepairLastArgumentName(argumentNames);
            return argumentNames;
        }

        private static void RepairLastArgumentName(IList<string> arguments)
        {
            var lastArgument = arguments[arguments.Count - 1];
            arguments[arguments.Count - 1] = lastArgument.Remove(lastArgument.Length - 2);
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
