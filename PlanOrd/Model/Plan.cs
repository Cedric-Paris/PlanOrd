using System.Collections.Generic;

namespace PlanOrd.Model
{
    public class Plan
    {
        public PlanGraph Graph { get; private set; }
        public SortedDictionary<int, PlanNode> EventsNotInPlan { get; private set; }
    }
}
