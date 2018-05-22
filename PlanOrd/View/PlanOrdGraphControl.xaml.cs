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

            //-- TEMPORARY --
            /*graph = new Graph();
            //graph.Attr.BackgroundColor = Color.Green;

            graph.AddEdge("4244678324", "1", "3306765570");
            graph.AddEdge("3306765570", "2", "3306795361");
            graph.AddEdge("3306765570", "3", "4165284267");
            graph.AddEdge("4165284267", "4", "2357829804");
            graph.AddEdge("4165284267", "5", "2357829804");
            graph.AddEdge("4165284267", "6", "4165808555");
            graph.AddEdge("4165808555", "7", "2358354092");
            graph.AddEdge("2357829804", "8", "3037888174");
            graph.AddEdge("2357829804", "9", "3037888174");
            graph.AddEdge("3037888174", "10", "3037888174");
            graph.AddEdge("2357829804", "11", "3057232447");
            graph.AddEdge("3057232447", "12", "4383314829");
            graph.AddEdge("4383314829", "13", "3538842958");
            graph.AddEdge("3538842958", "14", "5039186655");
            graph.AddEdge("2357829804", "15", "3057232447");
            graph.AddEdge("3057232447", "16", "4383314829");
            graph.AddEdge("4383314829", "17", "1777291275");
            graph.AddEdge("3037888174", "18", "4769343948");
            graph.AddEdge("4769343948", "1");
            graph.AddEdge("1", "2");
            graph.AddEdge("2", "3");
            graph.AddEdge("3", "4");
            graph.AddEdge("4", "5");
            graphViewer.Graph = graph;*/
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
