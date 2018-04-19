using System.Windows;
using PlanOrd.View;

namespace PlanOrd
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Start(object sender, StartupEventArgs e)
        {
            var wnd = new MaquetteWindow();
            wnd.Show();
        }

    }
}
