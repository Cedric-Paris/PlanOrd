using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl.PlanOrdOverlay;
using PlanOrd.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PlanOrd.View
{
    /// <summary>
    /// Logique d'interaction pour PlanOrdGraphControl.xaml
    /// </summary>
    public partial class PlanOrdGraphControl : UserControl
    {
        #region Static fields and methods
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(Graph), typeof(PlanOrdGraphControl), new PropertyMetadata(null, GraphChangedCallback));

        public static readonly DependencyProperty SelectedIdProperty =
            DependencyProperty.Register("SelectedId", typeof(string), typeof(PlanOrdGraphControl), new PropertyMetadata(null, SelectedIdChangedCallback));

        private static void SelectedIdChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            PlanOrdGraphControl instance = (PlanOrdGraphControl)obj;
            instance.graphViewer.SelectNode(instance.SelectedId);
        }

        private static void GraphChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            PlanOrdGraphControl instance = (PlanOrdGraphControl)obj;
            instance.isGraphOutdated = true;
            instance.DisplayGraph();
        }
        #endregion

        /// <summary>
        /// Graph Msagl affiche par ce control
        /// </summary>
        public Graph Graph
        {
            get { return (Graph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        /// <summary>
        /// Id du noeud selectionne par l'utilisateur
        /// </summary>
        public string SelectedId
        {
            get { return (string)GetValue(SelectedIdProperty); }
            set { SetValue(SelectedIdProperty, value); }
        }

        private PlanOrdGraphViewer graphViewer;
        private bool isGraphOutdated = false;


        /// <summary>
        /// Constructeur
        /// </summary>
        public PlanOrdGraphControl()
        {
            InitializeComponent();

            graphViewer = new PlanOrdGraphViewer();
            graphViewer.Initialize(containerDockPanel, verticalScrollBar, horizontalScrollBar, NodeToFrameworkElement);
            graphViewer.RunLayoutAsync = true;
            graphViewer.LayoutStarted += LayoutStarted;
            graphViewer.LayoutComplete += LayoutConplete;
            graphViewer.SelectedNodeChanged += (s,e) => { SelectedId = graphViewer.SelectedNodeId; };
        }

        /// <summary>
        /// Lance le layout et l'affichage du graphe
        /// </summary>
        private void DisplayGraph()
        {
            if (!IsLoaded)
                return;
            isGraphOutdated = false;

            //Render graph
            graphViewer.Graph = Graph;
        }

        /// <summary>
        /// Appelee quand le layout du graphe commence
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        private void LayoutStarted(object sender, EventArgs e)
        {
            if(graphViewer.Graph != null)
                renderingMessage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Appelee quand le layout du graphe est termine
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        private void LayoutConplete(object sender, EventArgs e)
        {
            renderingMessage.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Cree les objets graphiques a afficher pour un noeud donne
        /// </summary>
        /// <param name="node">Noeud a representer graphiquement</param>
        /// <returns>PlanNodeView</returns>
        private PlanNodeView NodeToFrameworkElement(Microsoft.Msagl.Drawing.Node node)
        {
            if (node.UserData == null)
                return null;

            if (node.UserData is PlanNodeViewModel)
                (node.UserData as PlanNodeViewModel).GraphViewer = graphViewer;

            var planNodeView = new PlanNodeView(node.UserData);
            planNodeView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            planNodeView.Width = planNodeView.DesiredSize.Width;
            planNodeView.Height = planNodeView.DesiredSize.Height;
            return planNodeView;
        }

        /// <summary>
        /// Appele lorsque le graph control est charge dans un fenetre
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        private void GraphControlLoaded(object sender, RoutedEventArgs e)
        {
            if (isGraphOutdated)
                DisplayGraph();
        }

        /// <summary>
        /// Effectue un zoom avant
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        private void ZoomIn(object sender, RoutedEventArgs e)
        {
            graphViewer.ZoomInCanvas();
        }

        /// <summary>
        /// Effectue un zoom arriere
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        private void ZoomOut(object sender, RoutedEventArgs e)
        {
            graphViewer.ZoomOutCanvas();
        }

        /// <summary>
        /// Ajuste la taille du graphe pour qu'il soit visible entierement a l'ecran
        /// </summary>
        private void FitGraphToView(object sender, RoutedEventArgs e)
        {
            graphViewer.FitGraphToView();
        }

        /// <summary>
        /// Ajuste la largeur du graphe pour qu'il remplisse entierement la largeur du GraphControl et
        /// scroll pour que le haut du graphe soit visible a l'ecran
        /// </summary>
        private void FitGraphWidthToViewAndGoTop(object sender, RoutedEventArgs e)
        {
            graphViewer.FitGraphWidthToViewAndGoTop();
        }
    }
}
