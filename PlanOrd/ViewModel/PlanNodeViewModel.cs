using Microsoft.Msagl.WpfGraphControl.PlanOrdOverlay;
using PlanOrd.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace PlanOrd.ViewModel
{
    /// <summary>
    /// Represente l'etat et les donnees d'un noeud du plan affiche a l'ecran
    /// </summary>
    public class PlanNodeViewModel : ViewModel
    {
        private static readonly Color defaultForeground = Colors.Black;
        private static readonly Color defaultBackground = Colors.White;
        private const double inactiveNodeOpacity = 0.3;

        private PlanNode node;
        private bool isActive;
        private bool isBanned;
        private string selectedCriteriaName;
        private Color foregroundColor;
        private Color backgroundColor;
        private double opacity;


        public int Id { get { return node.Id; } }

        public string Label { get { return node.Label; } }

        /// <summary>
        /// Le graphViewer dans lequel ce node est affiche. (null si non affiche)
        /// </summary>
        public PlanOrdGraphViewer GraphViewer { get; set; }

        /// <summary>
        /// Indique si le noeud a deja ete execute
        /// </summary>
        public bool IsRunned { get; set; }

        /// <summary>
        /// Indique si le noeud est actif dans l'interface graphique
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
                Opacity = isBanned || !isActive ? inactiveNodeOpacity : 1;
            }
        }
        /// <summary>
        /// Indique si le noeud a ete banni du plan par l'utilisateur
        /// </summary>
        public bool IsBanned
        {
            get { return isBanned; }
            set
            {
                isBanned = value;
                OnPropertyChanged("IsBanned");
                Opacity = isBanned || !isActive ? inactiveNodeOpacity : 1;
            }
        }

        /// <summary>
        /// Nom du critere selectionne dans l'interface
        /// </summary>
        public string SelectedCriteriaName
        {
            get { return selectedCriteriaName; }
            set
            {
                if(selectedCriteriaName != value)
                {
                    string last = selectedCriteriaName;
                    selectedCriteriaName = value;
                    UpdateCriteriasState(last);
                }
            }
        }

        public IList<CriteriaViewModel> Criterias { get; private set; }

        /// <summary>
        /// Opacite du noeud lorsqu'il est affiche (1 = 100%)
        /// </summary>
        public double Opacity
        {
            get { return opacity; }
            private set
            {
                if (opacity != value)
                {
                    opacity = value;
                    UpdateForegroundBrush();
                    UpdateBackgroundBrush();
                    OnPropertyChanged("Opacity");
                }
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="node">Node du modele</param>
        public PlanNodeViewModel(PlanNode node)
        {
            this.node = node;
            isActive = true;
            opacity = 1;
            foregroundColor = defaultForeground;
            backgroundColor = defaultBackground;

            Criterias = new List<CriteriaViewModel>();
            foreach(var crit in node.Criterias)
                Criterias.Add(new CriteriaViewModel(crit.Key, crit.Value));
        }

        /// <summary>
        /// Met a jour l'etat des CriteriaViewModels en fonction du critere selectionne
        /// </summary>
        private void UpdateCriteriasState(string lastValue = null)
        {
            CriteriaViewModel crit;
            if (lastValue != null && (crit = Criterias.FirstOrDefault(c => c.Name == lastValue)) != null)
            {
                crit.IsSelected = false;
            }

            if (SelectedCriteriaName != null && (crit = Criterias.FirstOrDefault(c => c.Name == SelectedCriteriaName)) != null)
            {
                crit.IsSelected = true;
                foregroundColor = crit.Color;
                UpdateForegroundBrush();
            }
            else if(foregroundColor != defaultForeground)
            {
                foregroundColor = defaultForeground;
                UpdateForegroundBrush();
            }
        }

        /// <summary>
        /// Met a jour la couleur du noeud pour qu'elle puisse s'afficher dans l'interface
        /// </summary>
        private void UpdateForegroundBrush()
        {
            if(GraphViewer != null)
            {
                Color c = new Color()
                {
                    A = (byte)(Opacity * 255),
                    R = foregroundColor.R,
                    G = foregroundColor.G,
                    B = foregroundColor.B,
                };
                GraphViewer.SetNodeForeground(Id.ToString(), c);
            }
        }

        /// <summary>
        /// Met a jour la couleur de fond du noeud pour qu'elle puisse s'afficher dans l'interface
        /// </summary>
        private void UpdateBackgroundBrush()
        {
            if (GraphViewer != null)
            {
                Color c = new Color()
                {
                    A = (byte)(Opacity * 255),
                    R = backgroundColor.R,
                    G = backgroundColor.G,
                    B = backgroundColor.B,
                };
                GraphViewer.SetNodeBackground(Id.ToString(), c);
            }
        }
    }
}
