using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic;
using BusinessLogic.Exceptions;
using BusinessLogic.Helpers;
using BusinessLogic.KMeans;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ViewModels.Providers;

namespace ViewModels
{
    public class PrepareDataViewModel : ViewModelBase
    {
        private readonly IOpenFileDialogProvider _openFileDialogProvider;
        private readonly IMessageBoxProvider _messageBoxProvider;
        private readonly StringToNumberConverter _stringToNumberConverter;

        private RelayCommand _browseFileCommand;
        private RelayCommand _fillDataCommand;
        private string _contentFileUrl;
        private string _roughSetsFileContent;
        private string _attributesDescription;

        public RelayCommand BrowseFileCommand => _browseFileCommand ?? (_browseFileCommand = new RelayCommand(BrowseFile));
        public RelayCommand FillDataCommand => _fillDataCommand ?? (_fillDataCommand = new RelayCommand(FillData));
        public string DescritpionFileUrl => ContentFileUrl.Insert(ContentFileUrl.Length - 4, "_descr");

        public string ContentFileUrl
        {
            get { return _contentFileUrl; }
            set
            {
                _contentFileUrl = value;
                RaisePropertyChanged();
            }
        }

        public List<DataObject> DataObjects { get; private set; } 

        public RoughSetInformations RoughSetInformations { get; private set; }

        public PrepareDataViewModel(IOpenFileDialogProvider openFileDialogProvider, IMessageBoxProvider messageBoxProvider)
        {
            _openFileDialogProvider = openFileDialogProvider;
            _messageBoxProvider = messageBoxProvider;
            _stringToNumberConverter = new StringToNumberConverter();
            DataObjects = new List<DataObject>();
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
                var l = new Lol();
                l.Clustering(RoughSetInformations, DataObjects);
            }
            catch (Exception exception)
            {
                _messageBoxProvider.ShowMessage(exception.Message);
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
            DataObjects.Clear();
            var stringSeparators = new[] { "\r\n" };
            var lines = _roughSetsFileContent.Substring(0, _roughSetsFileContent.Length - 2).Split(stringSeparators, StringSplitOptions.None);
            _stringToNumberConverter.ConvertStringsToNumbers(lines);
            FillDataObjectsList(lines);
        }

        private void FillDataObjectsList(IEnumerable<string> lines)
        {
            try
            {
                foreach (var argumentsValuesCollection in lines.Select(line => line.Split(',')))
                {
                    DataObjects.Add(new DataObject
                    {
                        Decision = double.Parse(argumentsValuesCollection.Last()),
                        Arguments = argumentsValuesCollection.Take(argumentsValuesCollection.Length - 1)
                            .Select(l => double.Parse(l.Replace('.', ','))).ToList()
                    });
                }
            }
            catch (Exception exception)
            {
                throw new FillDataObjectsListException(exception);
            }
        }

        private static List<string> PrepareArgumentNames(IReadOnlyList<string> lines)
        {
            var decisionClasses = new List<string>();

            for (var i = 1; i < lines.Count; i++)
            {
                decisionClasses.Add(lines[i].Split(':')[0]);
            }
            return decisionClasses;
        }

        private static List<string> PrepareDecisionClasses(IReadOnlyList<string> lines)
        {
            if (lines == null || lines.Count == 0)
                return new List<string>();

            var argumentNames = lines[0].Split(',').ToList();
            return argumentNames;
        }

        private void CreateRoughSetInformations(List<string> argumentNames, List<string> decisionClasses)
        {
            RoughSetInformations = new RoughSetInformations(argumentNames, decisionClasses);
        }
    }
}
