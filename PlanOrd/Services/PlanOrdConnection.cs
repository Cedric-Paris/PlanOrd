using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace PlanOrd.Services
{
    public class PlanOrdConnection
    {
        private readonly HttpClient client;

        public PlanOrdConnection()
        {
            client = new HttpClient();
        }

        public async Task<string> GetPlanAsync()
        {
            /*HttpResponseMessage m = await client.GetAsync('http://localhost/plan.json');
            return await m.Content.ReadAsStringAsync();*/

            string b = await Task.Run(() =>
            {
                return File.ReadAllText("./plan.json");
            });
            return b;
        }
    }
}
