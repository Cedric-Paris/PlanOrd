using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlanOrd.View
{
    /// <summary>
    /// Logique d'interaction pour MaquetteWindow.xaml
    /// </summary>
    public partial class MaquetteWindow : Window
    {
        public MaquetteWindow()
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
