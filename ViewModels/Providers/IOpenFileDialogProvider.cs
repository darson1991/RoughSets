namespace ViewModels.Providers
{
    public interface IOpenFileDialogProvider
    {
        string SelectedPath { get; set; }
        string DefaultExtension { get; set; }
        string Filter { get; set; }
        void ExecuteOpenFileDialog();
    }
}
