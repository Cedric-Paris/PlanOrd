using System.Windows;

namespace PlanOrd
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Start(object sender, StartupEventArgs e)
        {
            MainWindow wnd = new MainWindow();
            wnd.Show();
        }

    }
}
