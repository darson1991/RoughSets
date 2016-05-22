using GalaSoft.MvvmLight.Command;

namespace ViewModels
{
    public class MainViewModel
    {
        private RelayCommand _browseFileCommand;

        public RelayCommand BrowseFileCommand => _browseFileCommand ?? (_browseFileCommand = new RelayCommand(BrowseFile));

        private static void BrowseFile()
        {

        }
    }
}
