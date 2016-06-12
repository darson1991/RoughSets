using System;
using System.Windows.Input;
using ViewModels;

namespace RoughSets
{
    public partial class SearchingRoughSetWindow
    {
        public SearchingRoughSetWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var prepareDataViewModel = DataContext as SearchingRoughSetViewModel;
            if (prepareDataViewModel != null)
                prepareDataViewModel.GoToResultsPageAction += () => NavigationService?.Navigate(new Uri("ResultsWindow.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
