using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanOrd.ViewModel
{

    class PlanRunner
    {
        public IDictionary<int, PlanNodeViewModel> Nodes { get; private set; }
        public PlanNodeViewModel StartNode { get; private set; }
        public Microsoft.Msagl.WpfGraphControl.PlanOrdOverlay.PlanOrdGraphViewer Viewer { get; private set; }

        public PlanRunner(Microsoft.Msagl.WpfGraphControl.PlanOrdOverlay.PlanOrdGraphViewer viewer, IDictionary<int, PlanNodeViewModel> nodes, PlanNodeViewModel start)
        {
            Viewer = viewer;
            StartNode = start;
            Nodes = nodes;

        }

        public void Start()
        {
            RunNode(StartNode);
        }

        public void OnNodeRunDone(PlanNodeViewModel finishedNode)
        {
            foreach (int child in finishedNode.Node.Children.Keys)
            {
                bool launch = true;
                foreach (int childFather in finishedNode.Node.Children[child].Fathers.Keys)
                {
                    if (!Nodes[childFather].IsRunDone)
                    {
                        launch = false;
                        break;
                    }
                }
                if (launch)
                {
                    RunNode(Nodes[child]);
                }
            }
        }

        public async Task RunNode(PlanNodeViewModel node)
        {
            node.IsRunned = true;
            foreach (int child in node.Node.Children.Keys)
            {
                Viewer.AnimateEdge(node.Id.ToString(), child.ToString(), PlanNodeViewModel.DefaultForeground, PlanNodeViewModel.RunnedColor, node.Node.Duration);
            }
            await Task.Delay(node.Node.Duration * 1000);
            node.IsRunDone = true;
            OnNodeRunDone(node);
        }
    }
}
