using System.Collections.Generic;

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
