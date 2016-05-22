using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ViewModels.MvvmUtils
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>(true);
        }

        public static void Cleanup()
        {
        }
    }
}