using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RoughSets.MvvmUtils.Providers;
using ViewModels;
using ViewModels.Providers;

namespace RoughSets.MvvmUtils
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(CreateOpenFIleDialogProvider);

            SimpleIoc.Default.Register<MainViewModel>(true);
        }

        private static IOpenFileDialogProvider CreateOpenFIleDialogProvider()
        {
            return new OpenFileDialogProvider();
        }
    }
}