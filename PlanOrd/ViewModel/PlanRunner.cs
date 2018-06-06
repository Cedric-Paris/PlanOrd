using Microsoft.Msagl.WpfGraphControl.PlanOrdOverlay;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanOrd.ViewModel
{
    /// <summary>
    /// Gere le run du plan
    /// </summary>
    class PlanRunner
    {
        private IDictionary<int, PlanNodeViewModel> plaNodes;
        private PlanNodeViewModel startNode;
        private PlanOrdGraphViewer viewer;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="viewer">Viewer dans lequel les noeuds sont affiches</param>
        /// <param name="nodes">Liste des noeuds du plan</param>
        /// <param name="start">Noeud de depart du plan</param>
        public PlanRunner(PlanOrdGraphViewer viewer, IDictionary<int, PlanNodeViewModel> nodes, PlanNodeViewModel start)
        {
            this.viewer = viewer;
            startNode = start;
            this.plaNodes = nodes;
        }

        /// <summary>
        /// Lance le run du plan
        /// </summary>
        public void Start()
        {
            RunNode(startNode);
        }

        /// <summary>
        /// Callback quand un noeud a fini de runner
        /// </summary>
        /// <param name="finishedNode">ViewModel du noeud qui a fini de runner</param>
        private void OnNodeRunDone(PlanNodeViewModel finishedNode)
        {
            foreach (int child in finishedNode.Node.Children.Keys)
            {
                bool launch = true;
                foreach (int childFather in finishedNode.Node.Children[child].Fathers.Keys)
                {
                    if (!plaNodes[childFather].IsRunDone)
                    {
                        launch = false;
                        break;
                    }
                }
                if (launch)
                {
                    RunNode(plaNodes[child]);
                }
            }
        }

        /// <summary>
        /// Lance le run du noeud passe en parametre
        /// </summary>
        /// <param name="node">ViewModel du noeud a runner</param>
        /// <returns>Task (methode asynchrone)</returns>
        private async Task RunNode(PlanNodeViewModel node)
        {
            node.IsRunned = true;
            foreach (int child in node.Node.Children.Keys)
            {
                viewer.AnimateEdge(node.Id.ToString(), child.ToString(), PlanNodeViewModel.DefaultForeground, PlanNodeViewModel.RunnedColor, node.Node.Duration);
            }
            await Task.Delay(node.Node.Duration * 1000);
            node.IsRunDone = true;
            OnNodeRunDone(node);
        }
    }
}
