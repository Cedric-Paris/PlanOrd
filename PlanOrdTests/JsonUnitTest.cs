using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanOrd.Model;
using PlanOrd.Services;
using System.IO;

namespace PlanOrdTests
{
    [TestClass]
    public class JsonUnitTest
    {
        [TestMethod]
        public void CreatePlanFromStringTest()
        {
            bool eventRaised = false;
            string json = File.ReadAllText(@"./../../plan.json");

            JsonPlanProvider pp = new JsonPlanProvider();
            pp.PlanReady += (s, e) => { eventRaised = true; };
            pp.CreatePlanFromString(json);

            Plan plan = pp.Plan;
            Assert.IsTrue(eventRaised);
            Assert.IsNotNull(plan);

            // Tests sur les caracs
            Assert.AreEqual(plan.GlobalCharacteristics.Count, 1);
            Assert.AreEqual(plan.GlobalCharacteristics["difficulty"], 1);

            // Tests sur le graphe
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(plan.Graph.Nodes[i].Id, i);
            }
            Assert.AreEqual(plan.Graph.Nodes[0].Duration, 0);
            Assert.AreEqual(plan.Graph.Nodes[0].Fathers.Count, 0);
            Assert.AreEqual(plan.Graph.Nodes[0].Children.Count, 1);
            Assert.AreEqual(plan.Graph.Nodes[0].Children[1].Label, "CallMedivac()");
            Assert.AreEqual(plan.Graph.Nodes[0].Label, "Start");

            Assert.AreEqual(plan.Graph.Nodes[2].Duration, 1);
            Assert.AreEqual(plan.Graph.Nodes[2].Fathers.Count, 1);
            Assert.AreEqual(plan.Graph.Nodes[2].Children.Count, 1);
            Assert.AreEqual(plan.Graph.Nodes[2].Label, "add_victim()");

            Assert.AreEqual(plan.Graph.Nodes[5].Duration, 1);
            Assert.AreEqual(plan.Graph.Nodes[5].Fathers.Count, 1);
            Assert.AreEqual(plan.Graph.Nodes[5].Fathers[4].Label, "MedivacLands()");
            Assert.AreEqual(plan.Graph.Nodes[5].Children.Count, 0);
            Assert.AreEqual(plan.Graph.Nodes[5].Label, "EvacuateVictims()");
        }
    }
}
