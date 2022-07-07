using AnimeInformation.UserControls;
using System.Windows;
using System.Windows.Input;

namespace AnimeInformation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MinimiezeEvent_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ExitEvent_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is GridControl gridControl && DataContext is MVVM.MainViewModel mainViewModel)
                mainViewModel.GridViewModel = gridControl.DataContext as MVVM.GridViewModel;
        }

        private void InfoControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is InfoControl infoControl && DataContext is MVVM.MainViewModel mainViewModel)
                mainViewModel.InfoViewModel = infoControl.DataContext as MVVM.InfoViewModel;
        }
    }
}
