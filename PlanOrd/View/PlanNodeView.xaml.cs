using System.Windows.Controls;

namespace PlanOrd.View
{
    /// <summary>
    /// Logique d'interaction pour PlanNodeView.xaml
    /// </summary>
    public partial class PlanNodeView : DockPanel
    {
        public PlanNodeView(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}
