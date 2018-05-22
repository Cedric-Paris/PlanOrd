using System.Net.Http;
using System.Threading.Tasks;

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
            string b = await Task.Run(() => { return @"{

    'nodes': [{
		'v': 0,
		'criteria': {},
		'params': {
			'controlled': true,
			'delay': 0,
			'label': 'Start',
			'status': 'notstarted'
		}
	}, {
		'v': 1,
		'criteria': {'Another one': 4},
		'params': {
			'controlled': false,
			'duration': 1,
			'label': 'CallMedivac()',
			'status': 'notstarted'
		}
	}, {
		'v': 2,
		'criteria': {
			'difficulty': 1,
            'Criticity': 5,
            'Another one': 10,
            'Another': 8
		},
		'params': {
			'controlled': false,
			'duration': 1,
			'label': 'add_victim()',
			'status': 'notstarted'
		}
	}, {
		'v': 3,
		'criteria': {},
		'params': {
			'controlled': true,
			'duration': 1,
			'label': 'medivac_arriving()',
			'status': 'notstarted'
		}
	}, {
		'v': 4,
		'criteria': {},
		'params': {
			'controlled': false,
			'duration': 1,
			'label': 'MedivacLands()',
			'status': 'notstarted'
		}
	}, {
		'v': 5,
		'criteria': {},
		'params': {
			'controlled': false,
			'duration': 1,
			'label': 'EvacuateVictims()',
			'status': 'notstarted'
		}
	}],
	'edges': [{
		'v': 0,
		'w': 1
	}, {
		'v': 1,
		'w': 2
	}, {
		'v': 1,
		'w': 3
	}, {
		'v': 2,
		'w': 4
	}, {
		'v': 3,
		'w': 4
	}, {
		'v': 4,
		'w': 5
	}]
}".Replace("'", "\""); });
            return b;
        }
    }
}
