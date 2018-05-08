using Newtonsoft.Json;
using PlanOrd.Model;
using System;
using System.Collections.Generic;

namespace PlanOrd.Services
{
    public class JsonPlanProvider : IPlanProvider
    {
        public Plan Plan { get; private set; }

        public event EventHandler PlanReady;

        /// <summary>
        /// Creer un Plan a partir d'une chaine Json et le stocker dans le JsonPlanProvider.
        /// </summary>
        /// <param name="value">Chaine Json representant le plan</param>
        public void CreatePlanFromString(string value)
        {
            var jsonPlan = JsonConvert.DeserializeObject<JsonPlan>(value);
            var plan = new Plan();
            foreach (JsonNode jNode in jsonPlan.Nodes)
            {
                PlanNode pn = new PlanNode(jNode.Id);
                pn.Duration = jNode.Parameters.Duration;
                pn.Label = jNode.Parameters.Label;
                plan.Graph.Nodes.Add(jNode.Id, pn);

                foreach(string crit in jNode.Criteria.Keys)
                {
                    if (plan.GlobalCharacteristics.ContainsKey(crit))
                        plan.GlobalCharacteristics[crit] += jNode.Criteria[crit];
                    else
                        plan.GlobalCharacteristics.Add(crit, jNode.Criteria[crit]);                       
                }
            }
            foreach (JsonEdge e in jsonPlan.Edges)
            {
                plan.Graph.CreateArc(e.SourceId, e.TargetId);
            }

            Plan = plan;
            if (PlanReady != null)
                PlanReady(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Classe support pour la deserialization
    /// </summary>
    internal class JsonPlan
    {
        public List <JsonNode> Nodes { get; set; }
        public List <JsonEdge> Edges { get; set; }
    }

    internal class JsonNode
    {
        [JsonProperty(PropertyName = "v")]
        public int Id { get; set; }

        public Dictionary<string, int> Criteria { get; set; }

        [JsonProperty(PropertyName = "params")]
        public JsonNodeParams Parameters { get; set; }
    }

    internal class JsonEdge
    {
        [JsonProperty(PropertyName = "v")]
        public int SourceId { get; set; }
        [JsonProperty(PropertyName = "w")]
        public int TargetId { get; set; }
    }

    internal class JsonNodeParams
    {
        public bool Controlled { get; set; }
        public int Duration { get; set; }
        public string Label { get; set; }
        public string Status { get; set; }
    }
}
