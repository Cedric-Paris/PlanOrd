using System.Collections.Generic;

namespace PlanOrd.Model
{
    public class PlanGraph
    {
        public SortedDictionary<int, PlanNode> Nodes { get; private set; }
        
        /// <summary>
        /// constructeur
        /// </summary>
        public PlanGraph()
        {
            Nodes = new SortedDictionary<int, PlanNode>();
        }

        /// <summary>
        /// Créer un arc entre deux noeuds (existants) du graphe
        /// </summary>
        /// <param name="sourceId">Noeud source qui sera le pere du noeud cible</param>
        /// <param name="targetId">Noeud cible qui sera fils du noeud source</param>
        public void CreateArc(int sourceId, int targetId)
        {
            PlanNode source, target;
            if (Nodes.TryGetValue(sourceId, out source) && Nodes.TryGetValue(targetId, out target))
            {
                target.Fathers.Add(source.Id, source);
                source.Children.Add(target.Id, target);
            }
        }
    }
}
