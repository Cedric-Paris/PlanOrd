using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="sourceId">le noeud source qui sera le père du noeud cible</param>
        /// <param name="targetId">le noeud cible qui sera fils du noeud source</param>
        public void CreateArc(int sourceId, int targetId)
        {

            //a compléter

        }
    }
}
