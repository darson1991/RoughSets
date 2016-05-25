using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RoughSets.MvvmUtils.Providers;
using ViewModels;
using ViewModels.Providers;

namespace RoughSets.MvvmUtils
{
    public class ViewModelLocator
    {
        public PrepareDataViewModel PrepareDataViewModel => ServiceLocator.Current.GetInstance<PrepareDataViewModel>();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(CreateOpenFIleDialogProvider);
            SimpleIoc.Default.Register(CreateMessageBoxProvider);

            SimpleIoc.Default.Register<PrepareDataViewModel>(true);
        }

        private static IOpenFileDialogProvider CreateOpenFIleDialogProvider()
        {
            return new OpenFileDialogProvider();
        }

        private static IMessageBoxProvider CreateMessageBoxProvider()
        {
            return new MessageBoxProvider();
        }
    }
}