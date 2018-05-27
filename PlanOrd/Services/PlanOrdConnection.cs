﻿using System.Net.Http;
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
	'manual_events': [{
		'duration': 10,
		'label': 'event1',
		'predecessor': [1, 2],
		'status': 'notstarted'
	}, {
		'duration': 20,
		'label': 'Event2',
		'predecessor': [4],
		'status': 'notstarted'
	}],
	'nodes': [{
		'v': 0,
		'criteria': {},
		'params': {
			'controlled': true,
			'delay': 0,
			'label': 'Start',
			'state': 'notstarted'
		}
	}, {
		'v': 1,
		'criteria': {},
		'params': {
			'controlled': false,
			'duration': 1,
			'label': 'CallMedivac()',
			'status': 'notstarted'
		}
	}, {
		'v': 2,
		'criteria': {
			'difficulty': 1
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
	'habilites': [{
		'nodes': [1, 2, 3],
		'name': 'Communication'
	}, {
		'nodes': [3, 4],
		'name': 'Leadership'
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
