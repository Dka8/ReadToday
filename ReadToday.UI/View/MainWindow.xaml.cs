using System;
using System.Windows;
using ReadToday.UI.ViewModel;

namespace ReadToday.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CMainWindowModel _viewModel;

        public MainWindow(CMainWindowModel viewModel)
        {
            InitializeComponent();
            this.Loaded += OnLoaded;

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void OnLoaded(Object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }
    }
}
