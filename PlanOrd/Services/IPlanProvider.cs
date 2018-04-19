using PlanOrd.Model;
using System;

namespace PlanOrd.Services
{
    public interface IPlanProvider
    {
        Plan Plan { get; }

        event EventHandler PlanReady;

        void CreatePlanFromString(string value);
    }
}
