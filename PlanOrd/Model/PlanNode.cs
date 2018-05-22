using System.Collections.Generic;

namespace PlanOrd.Model
{
    public class PlanNode
    {
        public int Id {get; private set;}
        public string Label { get; set; }
        public int Duration { get; set; }
        public SortedDictionary<int, PlanNode> Fathers { get; private set; }
        public SortedDictionary<int, PlanNode> Children { get; private set; }
        //public SortedDictionary<int, PlanNode> CanBeLinkedWith; //en attente de specs
        public Dictionary<string, int> Criterias { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="IdNode">Id du noeud</param>
        public PlanNode(int IdNode)
        {
            this.Id = IdNode;
            Fathers = new SortedDictionary<int, PlanNode>();
            Children = new SortedDictionary<int, PlanNode>();
        }
    }
}
