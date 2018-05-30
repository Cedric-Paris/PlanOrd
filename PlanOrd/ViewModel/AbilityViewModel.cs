using System.Collections.Generic;

namespace PlanOrd.ViewModel
{
    public class AbilityViewModel : ViewModel
    {
        private bool isEnabled = true;

        /// <summary>
        /// Nom de l'habilite
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// List des IDs des noeuds composant l'habilite
        /// </summary>
        public List<int> NodeIds { get; private set; }

        /// <summary>
        /// Indique si l'habitite est activee
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if(isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="name">Nom de l'habilite</param>
        /// <param name="nodes">IDs des noeuds representant l'habilite</param>
        public AbilityViewModel(string name, List<int> nodes)
        {
            Name = name;
            NodeIds = nodes;
        }
    }
}
