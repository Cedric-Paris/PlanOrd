using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanOrd.Model
{
    public class Notion
    {
        public Dictionary<string, int> Characteristics { get; private set; }

        /// <summary>
        /// constructeur
        /// </summary>
        public Notion()
        {
            Characteristics = new Dictionary<string, int>();
        }

    }
}
