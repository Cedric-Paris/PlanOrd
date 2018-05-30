using System.Collections.Generic;

namespace PlanOrd.Model
{
    public class Plan
    {
        public PlanGraph Graph { get; private set; }
        public SortedDictionary<int, PlanNode> ManualEvents { get; private set; }
        public SortedDictionary<string, List<int>> Abilities { get; private set; }
        public SortedDictionary<string, int> PlanCriterias { get; private set;}

        /// <summary>
        /// constructeur
        /// </summary>
        public Plan()
        {
            Graph = new PlanGraph();
            ManualEvents = new SortedDictionary<int, PlanNode>();
            Abilities = new SortedDictionary<string, List<int>>();
            PlanCriterias = new SortedDictionary<string, int>();
        }

        /// <summary>
        /// Met à jour la liste des critere du plan
        /// La valeur d'un critere du plan est la somme des valeurs de ce critere pour chaque noeud
        /// </summary>
        public void UpdatePlanCriterias()
        {
            PlanCriterias.Clear();

            foreach(PlanNode node in Graph.Nodes.Values)
            {
                foreach(var critPair in node.Criterias)
                {
                    if (PlanCriterias.ContainsKey(critPair.Key))
                        PlanCriterias[critPair.Key] += critPair.Value;
                    else
                        PlanCriterias.Add(critPair.Key, critPair.Value);
                }
            }
        }
    }
}
