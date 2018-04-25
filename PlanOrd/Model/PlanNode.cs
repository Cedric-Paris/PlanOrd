using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanOrd.Model
{
    public class PlanNode
    {
        public int Id {get; private set;}
        public SortedDictionary<int, PlanNode> Father { get; private set; }
        public SortedDictionary<int, PlanNode> Children { get; private set; }
        //public SortedDictionary<int, PlanNode> CanBeLinkedWith; //en attente de specs
        public List<Notion> Notions { get; private set; }
        public Dictionary<string, int> GlobalCharacteristics { get; private set; }

        //constructeur
        public PlanNode(int IdNode)
        {
            this.Id = IdNode;
        }

        //met à jour la liste des caractéristiques du plan avec la valeur associée
        //cette liste des caractéristiques dépend ds caractéristiques de chaque notion ajouté
        //la valeur d'une caractéristique du plan est la somme des valeurs de cette caractérisque pour chaque notion
        public void UpdateGlobalCharacteristics()
        {
            //reset du dictionnaire actuel
            this.GlobalCharacteristics.Clear();

            //récupération de la liste des noms des caractéristiuques
            HashSet<string> CaracString = new HashSet<string>();
            foreach (Notion n in this.Notions)
            {
                foreach (KeyValuePair<string, int> charac in n.Characteristics) {
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
                            if (this.GlobalCharacteristics.ContainsKey(cs) == false) {
                                //on ajoute la caractéristique la première fois
                                this.GlobalCharacteristics.Add(cs, charac.Value); 
                            } else 
                            {
                                //on met ensuite à jour la valeur de la caractéristique pour les autres fois afin de faire la somme
                                this.GlobalCharacteristics[cs] = this.GlobalCharacteristics[cs] + charac.Value;
                            }
                            
                        }
                    }
                    
                }
            }
        }

    }
}
