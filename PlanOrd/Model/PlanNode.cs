using System.Collections.Generic;

namespace PlanOrd.Model
{
    public class PlanNode
    {
        public int Id {get; private set;}
        public string Label { get; set; }
        public int Duration { get; set; }
        public bool IsBanned { get; set; }
        public bool IsRunned { get; set; }
        public SortedDictionary<int, PlanNode> Fathers { get; private set; }
        public SortedDictionary<int, PlanNode> Children { get; private set; }
        public Dictionary<string, int> Criterias { get; set; }
        public List<int> AllowedPredecessors { get; set; }
        public List<int> AllowedSuccessors { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="IdNode">Id du noeud</param>
        public PlanNode(int IdNode)
        {
            this.Id = IdNode;
            IsBanned = false;
            IsRunned = false;
            Fathers = new SortedDictionary<int, PlanNode>();
            Children = new SortedDictionary<int, PlanNode>();
        }
    }
}
