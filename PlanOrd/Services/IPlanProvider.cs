using PlanOrd.Model;
using System.Threading.Tasks;

namespace PlanOrd.Services
{
    public interface IPlanProvider
    {
        Task<Plan> GetPlanAsync();

        Task ReplanAsync();
    }
}
