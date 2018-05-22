using System.Collections.Generic;
using System.Windows.Media;

namespace PlanOrd.ViewModel
{
    /// <summary>
    /// Represente l'etat et les donnees d'un critere affiche a l'ecran
    /// </summary>
    public class CriteriaViewModel : ViewModel
    {
        #region Static fields and methods
        private static readonly Color[] colors = { Colors.Blue, Colors.Red, Colors.Green, Colors.DarkCyan, Colors.YellowGreen };
        private static readonly IDictionary<string, Color> colorNameAssociation = new SortedDictionary<string, Color>();
        private static int currentColor;
        
        private static void RegisterCriteriaName(string criteriaName)
        {
            if (colorNameAssociation.ContainsKey(criteriaName))
                return;
            colorNameAssociation.Add(criteriaName, colors[currentColor]);
            currentColor = currentColor >= colors.Length - 1 ? 0 : currentColor + 1;
        }
        #endregion

        private int value;
        private bool isSelected;

        /// <summary>
        /// Nom du critere
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Valeur du critere
        /// </summary>
        public int Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                OnPropertyChanged("IntValue");
            }
        }

        /// <summary>
        /// Indique si le critere est selectionne dans l'interface
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Couleur associee au nom du critere
        /// </summary>
        public Color Color { get { return colorNameAssociation[Name]; } }

        /// <summary>
        /// Brush associee au nom du critere (interface)
        /// </summary>
        public Brush Brush { get { return new SolidColorBrush(colorNameAssociation[Name]); } }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="name">Nom du critere</param>
        /// <param name="value">Valeur du critere</param>
        public CriteriaViewModel(string name, int value = 0)
        {
            RegisterCriteriaName(name);
            Name = name;
            Value = value;
            IsSelected = false;
        }
    }
}
