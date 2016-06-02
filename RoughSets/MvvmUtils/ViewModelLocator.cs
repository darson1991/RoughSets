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
        public AlgorithmChoiceViewModel AlgorithmChoiceViewModel => ServiceLocator.Current.GetInstance<AlgorithmChoiceViewModel>();
        public SearchingRoughSetViewModel SearchingRoughSetViewModel => ServiceLocator.Current.GetInstance<SearchingRoughSetViewModel>();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //SimpleIoc.Default.Register(CreateNavigationService);
            SimpleIoc.Default.Register(CreateOpenFIleDialogProvider);
            SimpleIoc.Default.Register(CreateMessageBoxProvider);

            SimpleIoc.Default.Register<PrepareDataViewModel>(true);
            SimpleIoc.Default.Register<AlgorithmChoiceViewModel>(true);
            SimpleIoc.Default.Register<SearchingRoughSetViewModel>(true);
        }

        //private INavigationService CreateNavigationService()
        //{
        //    var navigationService = new GalaSoft.MvvmLight.Views.NavigationService();
        //    return navigationService;
        //}

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