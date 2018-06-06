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
        private const double inactiveNodeOpacity = 0.3;
        public static readonly Color DefaultForeground = Colors.Black;
        public static readonly Color DefaultBackground = Colors.White;
        public static readonly Color AbilitySelectedBackground = Colors.LemonChiffon;
        public static readonly Color RunnedColor = Colors.Green;

        #region Fields and properties
        private PlanOrdGraphViewer graphViewer;
        private bool isActive;
        private string selectedCriteriaName;
        private double opacity;
        private Color foregroundColor;
        private Color backgroundColor;


        public int Id { get { return Node.Id; } }

        public string Label { get { return Node.Label; } }

        public PlanNode Node { get; private set; }

        /// <summary>
        /// Le graphViewer dans lequel ce node est affiche. (null si non affiche)
        /// </summary>
        public PlanOrdGraphViewer GraphViewer { get; set; }

        /// <summary>
        /// Indique si le noeud a deja ete execute
        /// </summary>
        public bool IsRunned
        {
            get { return Node.IsRunned; }
            set
            {
                if(IsRunned != value)
                {
                    Node.IsRunned = value;
                    ForegroundColor = value ? RunnedColor : DefaultForeground;
                }
            }
        }

        /// <summary>
        /// Indique si le noeud a fini de runner
        /// </summary>
        public bool IsRunDone { get; set; }

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
                Opacity = IsBanned || !isActive ? inactiveNodeOpacity : 1;
            }
        }
        /// <summary>
        /// Indique si le noeud a ete banni du plan par l'utilisateur
        /// </summary>
        public bool IsBanned
        {
            get { return Node.IsBanned; }
            set
            {
                Node.IsBanned = value;
                OnPropertyChanged("IsBanned");
                Opacity = IsBanned || !isActive ? inactiveNodeOpacity : 1;
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
        /// Couleur du noeud (text et contour)
        /// </summary>
        public Color ForegroundColor
        {
            get { return foregroundColor; }
            private set
            {
                foregroundColor = value;
                UpdateForegroundBrush();
            }
        }

        /// <summary>
        /// Couleur de fond du noeud
        /// </summary>
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                UpdateBackgroundBrush();
            }
        }

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
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="node">Node du modele</param>
        public PlanNodeViewModel(PlanNode node)
        {
            Node = node;
            isActive = true;
            IsRunDone = false;
            opacity = Node.IsBanned ? inactiveNodeOpacity : 1;
            foregroundColor = DefaultForeground;
            backgroundColor = DefaultBackground;

            Criterias = new List<CriteriaViewModel>();
            if (node.Criterias != null)
            {
                foreach (var crit in node.Criterias)
                    Criterias.Add(new CriteriaViewModel(crit.Key, crit.Value));
            }
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
                ForegroundColor = crit.Color;
            }
            else
            {
                Color newForeground = IsRunned ? RunnedColor : DefaultForeground;
                if (ForegroundColor != newForeground)
                    ForegroundColor = newForeground;
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
                    R = ForegroundColor.R,
                    G = ForegroundColor.G,
                    B = ForegroundColor.B,
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
                    R = BackgroundColor.R,
                    G = BackgroundColor.G,
                    B = BackgroundColor.B,
                };
                GraphViewer.SetNodeBackground(Id.ToString(), c);
            }
        }
    }
}
