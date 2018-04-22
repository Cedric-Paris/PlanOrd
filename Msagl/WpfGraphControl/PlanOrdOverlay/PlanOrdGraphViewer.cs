using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Color = System.Windows.Media.Color;
using Point = Microsoft.Msagl.Core.Geometry.Point;
using WpfPoint = System.Windows.Point;

namespace Microsoft.Msagl.WpfGraphControl.PlanOrdOverlay
{
    /// <summary>
    /// Surcouche utilisee pour gerer les interactions avec le graphe dans le cadre du projet PlanOrd
    /// </summary>
    public class PlanOrdGraphViewer : GraphViewer
    {
        private const double ZoomPower = 0.9;
        private ScrollBar verticalScrollBar;
        private ScrollBar horizontalScrollBar;
        private Panel panelContainer;
        private bool ajustScroolView = true;
        private double verticalScrollValue = 0;
        private double horizontalScrollValue = 0;

        /// <summary>
        /// Initialize le viewer. Doit être appele avant toute interaction utilisateur
        /// </summary>
        /// <param name="panelContainer"></param>
        /// <param name="verticalScrollBar"></param>
        /// <param name="horizontalScrollBar"></param>
        public void Initialize(Panel panelContainer, ScrollBar verticalScrollBar, ScrollBar horizontalScrollBar, DrawingNodeToFrameworkElement drawingNodeToFrameworkEl = null)
        {
            BindToPanel(panelContainer);
            DrawingNodeToFrameworkEl = drawingNodeToFrameworkEl;
            this.verticalScrollBar = verticalScrollBar;
            this.horizontalScrollBar = horizontalScrollBar;
            this.panelContainer = panelContainer;
            this.verticalScrollBar.Scroll += OnScrollChanged;
            this.horizontalScrollBar.Scroll += OnScrollChanged;
            ViewChangeEvent += OnViewChange;
            LayoutComplete += OnViewChange;
        }

        /// <summary>
        /// Gere les modifications d'affichage lorsque l'utilisateur utilise la roulette
        /// - Met a jour la scrollbar vertical
        /// - Si la touche 'Shift' est maintenue, effectue un zoom
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">MouseWheelEventArgs</param>
        protected override void GraphCanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(ModifierKeys == Drawing.ModifierKeys.Shift)
            {
                base.GraphCanvasMouseWheel(sender, e);
            }
            else
            {
                verticalScrollBar.Value = verticalScrollBar.Value - e.Delta;
                OnScrollChanged(verticalScrollBar, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gere les modifications d'affichage lorsque l'utilisateur deplace la souris
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        protected override void GraphCanvasMouseMove(object sender, MouseEventArgs e)
        { }

        /// <summary>
        /// Appele quand les scrollbar sont deplacee par l'utilisateur.
        /// Ajuste la position du graphe par rapport a l'etat des scrollbars
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        private void OnScrollChanged(object sender, EventArgs e)
        {
            ajustScroolView = false;
            SetTransform(CurrentScale, CurrentXOffset - (horizontalScrollBar.Value - horizontalScrollValue), CurrentYOffset - (verticalScrollBar.Value - verticalScrollValue));
            verticalScrollValue = verticalScrollBar.Value;
            horizontalScrollValue = horizontalScrollBar.Value;
            ajustScroolView = true;
        }

        /// <summary>
        /// Appele quand l'affichage du graphe est modifie
        /// </summary>
        /// <param name="sender">Inutilise</param>
        /// <param name="e">Inutilise</param>
        private void OnViewChange(object sender, EventArgs e)
        {
            if(ajustScroolView)
                AdjustScrollViewState();
        }

        /// <summary>
        /// Ajuste la position des scrollbars par rapport a la position du graphe
        /// </summary>
        private void AdjustScrollViewState()
        {
            verticalScrollBar.Maximum = Math.Max(0, (GeomGraph.Height * CurrentScale) - panelContainer.ActualHeight);
            verticalScrollBar.ViewportSize = panelContainer.ActualHeight;
            verticalScrollBar.LargeChange = verticalScrollBar.ViewportSize * 0.60;
            verticalScrollBar.SmallChange = verticalScrollBar.ViewportSize / 40;
            horizontalScrollBar.Maximum = Math.Max(0, (GeomGraph.Width * CurrentScale) - panelContainer.ActualWidth);
            horizontalScrollBar.ViewportSize = panelContainer.ActualWidth;
            horizontalScrollBar.LargeChange = horizontalScrollBar.ViewportSize * 0.60;
            horizontalScrollBar.SmallChange = horizontalScrollBar.ViewportSize / 50;

            var rectTransform = RectToFillGraphBackground.RenderTransform as MatrixTransform;
            var rectFillOrigin = new WpfPoint(rectTransform.Matrix.OffsetX, rectTransform.Matrix.OffsetY);
            var rectPointZero = GraphCanvas.RenderTransform.Transform(rectFillOrigin);

            verticalScrollBar.Value = rectPointZero.Y * -1;
            horizontalScrollBar.Value = rectPointZero.X  * -1;
            verticalScrollValue = verticalScrollBar.Value;
            horizontalScrollValue = horizontalScrollBar.Value;
        }

        /// <summary>
        /// S'assure que le graphe ne sorte pas de la zone visible a l'ecran.
        /// Verifie les valeurs qui seront appliquees sur le transform du Canvas et corrige les offsets si besoin.
        /// </summary>
        /// <param name="scale">Scale</param>
        /// <param name="dx">X offset</param>
        /// <param name="dy">Y offset</param>
        /// <returns>Offsets corriges</returns>
        private Tuple<double, double> CheckOffsetValidity(double scale, double dx, double dy)
        {
            double verticalMaximum = Math.Max(0, (GeomGraph.Height * scale) - panelContainer.ActualHeight);
            double horizontalMaximum = Math.Max(0, (GeomGraph.Width * scale) - panelContainer.ActualWidth);

            var rectTransform = RectToFillGraphBackground.RenderTransform as MatrixTransform;
            var rectFillOrigin = new WpfPoint(rectTransform.Matrix.OffsetX, rectTransform.Matrix.OffsetY);
            var trans = new MatrixTransform(scale, 0, 0, -scale, dx, dy);
            var newPos = trans.Transform(rectFillOrigin);

            if (newPos.X > 0 && RectToFillGraphBackground.ActualWidth * scale > panelContainer.ActualWidth)
                dx -= newPos.X;
            else if (newPos.X * -1 > horizontalMaximum)
                dx += (newPos.X * -1) - horizontalMaximum;
            else if (RectToFillGraphBackground.ActualWidth * scale <= panelContainer.ActualWidth && newPos.X + RectToFillGraphBackground.ActualWidth * scale > panelContainer.ActualWidth)
                dx -= RectToFillGraphBackground.ActualWidth * scale + newPos.X - panelContainer.ActualWidth;

            if (newPos.Y > 0 && RectToFillGraphBackground.ActualHeight * scale > panelContainer.ActualHeight)
                dy -= newPos.Y;
            else if (newPos.Y * -1 > verticalMaximum)
                dy += (newPos.Y * -1) - verticalMaximum;
            else if (RectToFillGraphBackground.ActualHeight * scale <= panelContainer.ActualHeight && newPos.Y + RectToFillGraphBackground.ActualHeight * scale > panelContainer.ActualHeight)
                dy -= RectToFillGraphBackground.ActualHeight * scale + newPos.Y - panelContainer.ActualHeight;

            return new Tuple<double, double>(dx, dy);
        }

        /// <summary>
        /// Change le RenderTransform du Canvas en applicant un scale et des decalages en X, Y (offset X, Y).
        /// Les offsets sont corriges si ils depassent la zone visible.
        /// Lance l'event ViewChangeEvent.
        /// </summary>
        /// <param name="scale">New scale</param>
        /// <param name="dx">New offset x</param>
        /// <param name="dy">New offset y</param>
        protected override void SetTransform(double scale, double dx, double dy)
        {
            Tuple<double, double> offsets = CheckOffsetValidity(scale, dx, dy);
            base.SetTransform(scale, offsets.Item1, offsets.Item2);
        }

        /// <summary>
        /// Retourne le point correspondant au centre du GraphControl par rapport au Canvas
        /// </summary>
        /// <returns>Centre relativement au Canvas</returns>
        private WpfPoint GetRelativeCanvasCenter()
        {
            //Trouver le centre du panel contenant le Canvas du graphe
            var panelCenter = panelContainer.TransformToAncestor((FrameworkElement)panelContainer.Parent).Transform(new WpfPoint(panelContainer.ActualWidth / 2, panelContainer.ActualHeight / 2));
            // Le point sur l'ecran qui correspond au centre du panel
            var centerScreen = panelContainer.PointToScreen(panelCenter);
            // Recuperer le point correspondant au centre du panel relativement au canevas
            return GraphCanvas.PointFromScreen(centerScreen);
        }

        /// <summary>
        /// Effectue un zoom avant
        /// </summary>
        public void ZoomInCanvas()
        {
            if (panelContainer == null)
                return;
            WpfPoint canvasCenter = GetRelativeCanvasCenter();
            ZoomAbout(1.0 / ZoomPower * ZoomFactor, canvasCenter);
        }

        /// <summary>
        /// Effectue un zoom arriere
        /// </summary>
        public void ZoomOutCanvas()
        {
            if (panelContainer == null)
                return;
            WpfPoint canvasCenter = GetRelativeCanvasCenter();
            ZoomAbout(ZoomPower * ZoomFactor, canvasCenter);
        }

        /// <summary>
        /// Ajuste la taille du graphe pour qu'il soit visible entierement a l'ecran
        /// </summary>
        public void FitGraphToView()
        {
            if (GeomGraph == null)
                return;

            var scale = Math.Min(GraphCanvas.RenderSize.Width / GeomGraph.Width, GraphCanvas.RenderSize.Height / GeomGraph.Height);
            var graphCenter = GeomGraph.BoundingBox.Center;
            var vp = new Rectangle(new Point(0, 0),
                                   new Point(GraphCanvas.RenderSize.Width, GraphCanvas.RenderSize.Height));
            var dx = vp.Width / 2 - scale * graphCenter.X;
            var dy = vp.Height / 2 + scale * graphCenter.Y;

            SetTransform(scale, dx, dy);
        }

        /// <summary>
        /// Ajuste la largeur du graphe pour qu'il remplisse entierement la largeur du GraphControl et
        /// scroll pour que le haut du graphe soit visible a l'ecran
        /// </summary>
        public void FitGraphWidthToViewAndGoTop()
        {
            var customBoundingBox = new Rectangle(GeomGraph.BoundingBox.LeftTop, GeomGraph.BoundingBox.RightBottom);
            customBoundingBox.Height = 2;
            var graphCenter = customBoundingBox.Center;

            var scale = Math.Min(GraphCanvas.RenderSize.Width / (customBoundingBox.RightBottom.X - customBoundingBox.LeftTop.X),
                                 GraphCanvas.RenderSize.Height / customBoundingBox.Height);

            var viewPort = new Rectangle(new Point(0, 0), new Point(GraphCanvas.RenderSize.Width, GraphCanvas.RenderSize.Height));

            var heightDifference = (GeomGraph.BoundingBox.Height * scale) - viewPort.Height;

            var dx = viewPort.Width / 2 - scale * graphCenter.X;
            var dy = viewPort.Height / 2 + scale * graphCenter.Y + heightDifference / 2;
            SetTransform(scale, dx, dy);
        }

        
        public void SelectNodes(IEnumerable<Node> nodes)//, Drawing.Color SelectedNodeColor)
        {
            UnselectAllNodes();
            IViewerObject viewerObject;
            IViewerNode vNode;
            foreach(Node node in nodes)
            {
                if(this.drawingObjectsToIViewerObjects.TryGetValue(node, out viewerObject) && (vNode = viewerObject as IViewerNode) != null)
                {
                    //vNode.Node.Attr.Color = SelectedNodeColor;
                    LayoutEditor.SelectObjectForDragging(vNode);
                }
            }
        }

        /// <summary>
        /// Deselectionne tous les noeuds selectionnes dans le graphe
        /// </summary>
        public void UnselectAllNodes()
        {
            foreach (IViewerNode node in LayoutEditor.ObjectsSelectedForDrag)
                LayoutEditor.UnselectObjectForDragging(node);
        }

        /// <summary>
        /// Lance une animation sur un arc du graphe: l'arc passe progressivement de la couleur 'From' a la couleur 'To'
        /// </summary>
        /// <param name="edge">Reference sur l'arc a animer</param>
        /// <param name="From">Couleur initiale</param>
        /// <param name="To">Couleur en fin d'animation</param>
        /// <param name="duration">Duree necessaire pour changer la couleur de l'arc</param>
        /// <param name="colorTarget">Indique si le noeud target de l'arc doit etre colorie a la fin de l'animation</param>
        public void AnimateEdge(Edge edge, Color From, Color To, double duration, bool colorTarget = true)
        {
            IViewerObject iViewer;
            VEdge edgeToAnim;
            if (!drawingObjectsToIViewerObjects.TryGetValue(edge, out iViewer) || (edgeToAnim = iViewer as VEdge) == null)
                return;

            //Create gradient
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new WpfPoint(0.5, 0);
            gradient.EndPoint = new WpfPoint(0.5, 1);
            var gStopStart = new GradientStop(From, 1);
            var gStopEnd = new GradientStop(To, 1);
            gradient.GradientStops.Add(gStopStart);
            gradient.GradientStops.Add(gStopEnd);

            //Run animation
            edgeToAnim.CurvePath.Stroke = gradient;
            var animation = new DoubleAnimation(0, TimeSpan.FromSeconds(duration));
            animation.Completed += (s, args) =>
            {
                Brush b = new SolidColorBrush(To);
                edgeToAnim.CurvePath.Stroke = b;
                edgeToAnim.TargetArrowHeadPath.Stroke = b;
                edgeToAnim.TargetArrowHeadPath.Fill = b;
                VNode node;
                if (colorTarget && drawingObjectsToIViewerObjects.TryGetValue(edge.TargetNode, out iViewer) && (node = iViewer as VNode) != null)
                    node.BoundaryPath.Stroke = b;
            };

            gStopStart.BeginAnimation(GradientStop.OffsetProperty, animation);
            gStopEnd.BeginAnimation(GradientStop.OffsetProperty, animation);
        }
    }
}
