using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ViewModels.ViewModel;

namespace ViewModels.MvvmUtils
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static void Cleanup()
        {
        }
    }
}