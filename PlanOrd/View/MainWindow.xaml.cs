using System.Windows;

namespace PlanOrd.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMinimizeButtonClicked(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeClicked(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void OnRestoreButtonClicked(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
