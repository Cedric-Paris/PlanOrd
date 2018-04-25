using System.Collections.Generic;

namespace PlanOrd.Model
{
    public class Plan
    {
        public PlanGraph Graph { get; private set; }
        public SortedDictionary<int, PlanNode> EventsNotInPlan { get; private set; }

        public List<Notion> Notions { get; private set; }
        public Dictionary<string, int> GlobalCharacteristics { get; private set; }

        /// <summary>
        /// constructeur
        /// </summary>
        public Plan()
        {
            Graph = new PlanGraph();
            EventsNotInPlan = new SortedDictionary<int, PlanNode>();
            Notions = new List<Notion>();
            GlobalCharacteristics = new Dictionary<string, int>();
        }

        /// <summary>
        /// met à jour la liste des caractéristiques du plan avec la valeur associée
        /// cette liste des caractéristiques dépend des caractéristiques de chaque notion ajouté
        /// la valeur d'une caractéristique du plan est la somme des valeurs de cette caractérisque pour chaque notion
        /// </summary>
       /* public void UpdateGlobalCharacteristics()
        {
            //reset du dictionnaire actuel
            this.GlobalCharacteristics.Clear();

            //récupération de la liste des noms des caractéristiuques
            HashSet<string> CaracString = new HashSet<string>();
            foreach (Notion n in this.Notions)
            {
                foreach (KeyValuePair<string, int> charac in n.Characteristics)
                {
                    CaracString.Add(charac.Key);
                }
            }

            //récupération de la valeur globale des caractéristiques de chaque notion
            foreach (string cs in CaracString)
            {
                foreach (Notion n in this.Notions)
                {
                    foreach (KeyValuePair<string, int> charac in n.Characteristics)
                    {
                        if (cs.Equals(charac.Key))
                        {
                            if (this.GlobalCharacteristics.ContainsKey(cs) == false)
                            {
                                //on ajoute la caractéristique la première fois
                                this.GlobalCharacteristics.Add(cs, charac.Value);
                            }
                            else
                            {
                                //on met ensuite à jour la valeur de la caractéristique pour les autres fois afin de faire la somme
                                this.GlobalCharacteristics[cs] = this.GlobalCharacteristics[cs] + charac.Value;
                            }

                        }
                    }

                }
            }
        }*/
    }
}
