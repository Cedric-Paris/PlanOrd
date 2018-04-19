﻿using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl.PlanOrdOverlay;
using PlanOrd.Model;
using System.Windows;
using System.Windows.Controls;

namespace PlanOrd.View
{
    /// <summary>
    /// Logique d'interaction pour PlanOrdGraphControl.xaml
    /// </summary>
    public partial class PlanOrdGraphControl : UserControl
    {
        /*public static readonly DependencyProperty PlanOrdGraphProperty =
            DependencyProperty.Register("PlanOrdGraph", typeof(PlanGraph), typeof(PlanOrdGraphControl), null);

        public PlanGraph PlanGraph
        {
            get { return (PlanGraph)GetValue(PlanOrdGraphProperty); }
            set
            {
                SetValue(PlanOrdGraphProperty, value);
                currentPlanGraph = value;
                isGraphOutdated = true;
                DisplayGraph();
            }
        }*/

        private PlanOrdGraphViewer graphViewer;
        //private PlanGraph currentPlanGraph;
        private Graph graph;
        private bool isGraphOutdated = false;


        /// <summary>
        /// Constructeur
        /// </summary>
        public PlanOrdGraphControl()
        {
            InitializeComponent();
            graphViewer = new PlanOrdGraphViewer();
            graphViewer.Initialize(containerDockPanel, verticalScrollBar, horizontalScrollBar);
        }

        /// <summary>
        /// Lance le layout et l'affichage du graphe
        /// </summary>
        private void DisplayGraph()
        {
            if (!IsLoaded)
                return;
            isGraphOutdated = false;

            /*graph = new Graph();
            foreach(var nodePair in currentPlanGraph.Nodes)
            {
                Node n = graph.AddNode(nodePair.Key.ToString());
                n.UserData = nodePair.Value;
                n.LabelText = nodePair.Value.Label;
                foreach(var childNodePair in nodePair.Value.Children)
                {
                    Edge e = graph.AddEdge(nodePair.Key.ToString(), childNodePair.Key.ToString());
                }
            }

            //Render graph
            graphViewer.Graph = graph;*/
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
            graph = new Graph();
            graph.Attr.BackgroundColor = Color.Green;

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
            graphViewer.Graph = graph;
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