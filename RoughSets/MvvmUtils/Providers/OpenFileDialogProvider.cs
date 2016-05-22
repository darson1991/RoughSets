using Microsoft.Win32;
using ViewModels.Providers;

namespace RoughSets.MvvmUtils.Providers
{
    public class OpenFileDialogProvider : IOpenFileDialogProvider
    {
        public string SelectedPath { get; set; }
        public string DefaultExtension { get; set; }
        public string Filter { get; set; }

        public OpenFileDialogProvider()
        {
            DefaultExtension = ".txt";
            Filter = "TXT Files(*.txt) | *.txt";
        }

        public void ExecuteOpenFileDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();

            SelectedPath = dialog.FileName;
        }
    }
}
