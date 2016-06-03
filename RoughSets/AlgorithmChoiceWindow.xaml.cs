using System;
using System.Windows.Input;
using ViewModels;

namespace RoughSets
{
    public partial class AlgorithmChoiceWindow
    {
        public AlgorithmChoiceWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var prepareDataViewModel = DataContext as AlgorithmChoiceViewModel;
            if (prepareDataViewModel != null)
                prepareDataViewModel.GoToSearchingRoughSetPageAction += () => NavigationService?.Navigate(new Uri("SearchingRoughSetWindow.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
