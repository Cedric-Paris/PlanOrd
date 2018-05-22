using System.Windows;
using PlanOrd.View;
using PlanOrd.ViewModel;
using PlanOrd.Services;

namespace PlanOrd
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Start(object sender, StartupEventArgs e)
        {
            IPlanProvider provider = new JsonPlanProvider(new PlanOrdConnection());

            var viewModel = new MainWindowViewModel(provider);
            var wnd = new MainWindow();
            wnd.DataContext = viewModel;
            wnd.Show();

            viewModel.LaunchPlanRetrievingAsync();
        }

    }
}
