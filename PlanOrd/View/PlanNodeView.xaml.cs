using System.Windows.Controls;

namespace PlanOrd.View
{
    /// <summary>
    /// Representation graphique d'un noeud du plan
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
