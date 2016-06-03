using System;
using System.Windows.Input;
using ViewModels;

namespace RoughSets
{
    public partial class PrepareDataWindow
    {
        public PrepareDataWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var prepareDataViewModel = DataContext as PrepareDataViewModel;
            if (prepareDataViewModel != null)
                prepareDataViewModel.GoToAlgorithmChoicePageAction += () => NavigationService?.Navigate(new Uri("AlgorithmChoiceWindow.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
