using System.Windows;
using ViewModels.Providers;

namespace RoughSets.MvvmUtils.Providers
{
    public class MessageBoxProvider : IMessageBoxProvider
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
