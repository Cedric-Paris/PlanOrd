using Newtonsoft.Json;
using PlanOrd.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanOrd.Services
{
    public class JsonPlanProvider : IPlanProvider
    {
        private readonly PlanOrdConnection connection;

        public JsonPlanProvider(PlanOrdConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Plan> GetPlanAsync()
        {
            string jsonPlan = await connection.GetPlanAsync();
            return CreatePlanFromString(jsonPlan);
        }

        public async Task ReplanAsync()
        {
            await connection.GetPlanAsync();
        }

        /// <summary>
        /// Creer un Plan a partir d'une chaine Json et le stocker dans le JsonPlanProvider.
        /// </summary>
        /// <param name="value">Chaine Json representant le plan</param>
        /// <returns>Plan genere</returns>
        private Plan CreatePlanFromString(string value)
        {
            var jsonPlan = JsonConvert.DeserializeObject<JsonPlan>(value);
            var plan = new Plan();
            foreach (JsonNode jNode in jsonPlan.Nodes)
            {
                PlanNode pn = new PlanNode(jNode.Id);
                pn.Duration = jNode.Parameters.Duration;
                pn.Label = jNode.Parameters.Label;
                pn.Criterias = jNode.Criteria;
                plan.Graph.Nodes.Add(jNode.Id, pn);
            }
            foreach (JsonEdge e in jsonPlan.Edges)
            {
                plan.Graph.CreateArc(e.SourceId, e.TargetId);
            }

            int currentId = plan.Graph.Nodes.Values.Max(n => n.Id) + 1;
            foreach (ManualEvent mEvent in jsonPlan.ManualEvents)
            {
                PlanNode pn = new PlanNode(currentId++);
                pn.Duration = mEvent.Duration;
                pn.Label = mEvent.Label;
                pn.AllowedPredecessors = mEvent.Predecessor ?? new List<int>();
                pn.AllowedSuccessors = mEvent.Successor ?? new List<int>();
                plan.ManualEvents.Add(pn.Id, pn);
            }
            foreach (Hability h in jsonPlan.Habilities)
            {
                plan.Habilities.Add(h.Name, h.Nodes);
            }

            plan.UpdatePlanCriterias();

            return plan;
        }
    }

    /// <summary>
    /// Classe support pour la deserialization
    /// </summary>
    internal class JsonPlan
    {
        public List<JsonNode> Nodes { get; set; }
        public List<JsonEdge> Edges { get; set; }
        [JsonProperty(PropertyName = "manual_events")]
        public List<ManualEvent> ManualEvents { get; set; }
        [JsonProperty(PropertyName = "habilites")]
        public List<Hability> Habilities { get; set; }
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

    internal class Hability
    {
        public string Name { get; set; }
        public List<int> Nodes { get; set; }
    }

    internal class ManualEvent
    {
        public int Duration { get; set; }
        public string Label { get; set; }
        public List<int> Predecessor { get; set; }
        public List<int> Successor { get; set; }
        public string Status { get; set; }
    }
}
