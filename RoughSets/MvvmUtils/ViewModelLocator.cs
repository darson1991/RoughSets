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
        public ResultsViewModel ResultsViewModel => ServiceLocator.Current.GetInstance<ResultsViewModel>();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(CreateOpenFIleDialogProvider);
            SimpleIoc.Default.Register(CreateMessageBoxProvider);

            SimpleIoc.Default.Register<PrepareDataViewModel>(true);
            SimpleIoc.Default.Register<AlgorithmChoiceViewModel>(true);
            SimpleIoc.Default.Register<SearchingRoughSetViewModel>(true);
            SimpleIoc.Default.Register<ResultsViewModel>(true);
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