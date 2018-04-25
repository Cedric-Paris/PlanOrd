using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanOrd.Model
{
    public class PlanNode
    {
        public int Id {get; private set;}
        public SortedDictionary<int, PlanNode> Father { get; private set; }
        public SortedDictionary<int, PlanNode> Children { get; private set; }
        //public SortedDictionary<int, PlanNode> CanBeLinkedWith; //en attente de specs
        //notion et carac, utilisation à définir

        /// <summary>
        /// constructeur
        /// </summary>
        public PlanNode(int IdNode)
        {
            this.Id = IdNode;
            Father = new SortedDictionary<int, PlanNode>();
            Children = new SortedDictionary<int, PlanNode>();
        }

    }
}
