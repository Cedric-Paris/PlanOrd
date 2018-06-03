using Microsoft.Msagl.Drawing;
using PlanOrd.Model;
using PlanOrd.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PlanOrd.ViewModel
{
    public class MainWindowViewModel : ViewModel
    {
        #region Fields and properties
        private IPlanProvider planProvider;
        private Plan plan;
        private Graph msaglGraph;
        private string selectedIdAsString;
        private IDictionary<int, PlanNodeViewModel> nodeViewModels;
        private ObservableCollection<PlanNodeViewModel> nodesNotInPlan;
        private PlanNodeViewModel nodeSelectedInAddList;
        private IList<CriteriaViewModel> criterias;
        private CriteriaViewModel selectedCriteria;
        private IList<AbilityViewModel> abilities;
        private AbilityViewModel selectedAbility;
        private bool addBeforeButtonEnabled = false;
        private bool addAfterButtonEnabled = false;
        private Visibility banVisibility = Visibility.Collapsed;
        private Visibility unbanVisibility = Visibility.Collapsed;

        /// <summary>
        /// Representation Msagl du plan
        /// </summary>
        public Graph MsaglGraph
        {
            get { return msaglGraph; }
            set
            {
                msaglGraph = value;
                OnPropertyChanged("MsaglGraph");
            }
        }

        /// <summary>
        /// Id du noeud selectionne dans l'interface
        /// (sous forme de string car Msagl fonctionne avec des id string)
        /// </summary>
        public string SelectedIdAsString
        {
            get { return selectedIdAsString; }
            set
            {
                selectedIdAsString = value;
                OnNodeSelected();
                OnPropertyChanged("SelectedIdAsString");
            }
        }

        /// <summary>
        /// Liste des evenements ne faisant pas encore partie du plan (ceux a ajouter)
        /// </summary>
        public ObservableCollection<PlanNodeViewModel> NodesNotInPlan
        {
            get { return nodesNotInPlan; }
            set
            {
                nodesNotInPlan = value;
                OnPropertyChanged("NodesNotInPlan");
            }
        }

        /// <summary>
        /// Noeud selectionne dans la liste des noeuds a ajouter au graphe
        /// </summary>
        public PlanNodeViewModel NodeSelectedInAddList
        {
            get { return nodeSelectedInAddList; }
            set
            {
                nodeSelectedInAddList = value;
                OnNodesNotInPlanSelected();
                OnPropertyChanged("NodeSelectedInAddList");
            }
        }

        /// <summary>
        /// Liste des criteres du plan
        /// </summary>
        public IList<CriteriaViewModel> Criterias
        {
            get { return criterias; }
            set
            {
                criterias = value;
                OnPropertyChanged("Criterias");
            }
        }

        /// <summary>
        /// Critere selectionne dans l'interface (liste de droite)
        /// </summary>
        public CriteriaViewModel SelectedCriteria
        {
            get { return selectedCriteria; }
            set
            {
                selectedCriteria = value;
                OnCriteriaSelected();
                OnPropertyChanged("SelectedCriteria");
            }
        }

        /// <summary>
        /// Liste des habilites
        /// </summary>
        public IList<AbilityViewModel> Abilities
        {
            get { return abilities; }
            set
            {
                abilities = value;
                OnPropertyChanged("Abilities");
            }
        }

        /// <summary>
        /// Habilite selectionne dans l'interface (liste de droite)
        /// </summary>
        public AbilityViewModel SelectedAbility
        {
            get { return selectedAbility; }
            set
            {
                AbilityViewModel last = selectedAbility;
                selectedAbility = value;
                OnAbilitySelected(last);
                OnPropertyChanged("SelectedAbility");
            }
        }

        /// <summary>
        /// Commande executer lorsque l'utilisateur clic sur le bouton 'Lancer la seance'
        /// </summary>
        public ButtonCommand RunPlanCommand { get { return new ButtonCommand(RunPlan); } }

        /// <summary>
        /// Indique si le bouton d'ajout "avant" est active dans l'interface
        /// </summary>
        public bool AddBeforeButtonEnabled
        {
            get { return addBeforeButtonEnabled; }
            set
            {
                addBeforeButtonEnabled = value;
                OnPropertyChanged("AddBeforeButtonEnabled");
            }
        }

        public ButtonCommand AddBeforeCommand { get { return new ButtonCommand(AddBefore); } }

        /// <summary>
        /// Indique si le bouton d'ajout "apres" est active dans l'interface
        /// </summary>
        public bool AddAfterButtonEnabled
        {
            get { return addAfterButtonEnabled; }
            set
            {
                addAfterButtonEnabled = value;
                OnPropertyChanged("AddAfterButtonEnabled");
            }
        }

        public ButtonCommand AddAfterCommand { get { return new ButtonCommand(AddAfter); } }

        /// <summary>
        /// Visibilite du bouton 'Bannir' dans l'interface
        /// </summary>
        public Visibility BanVisibility
        {
            get { return banVisibility; }
            set
            {
                banVisibility = value;
                OnPropertyChanged("BanVisibility");
            }
        }

        public ButtonCommand BanCommand { get { return new ButtonCommand(Ban); } }

        /// <summary>
        /// Visibilite du bouton 'Debannir' dans l'interface
        /// </summary>
        public Visibility UnbanVisibility
        {
            get { return unbanVisibility; }
            set
            {
                unbanVisibility = value;
                OnPropertyChanged("UnbanVisibility");
            }
        }

        public ButtonCommand UnbanCommand { get { return new ButtonCommand(Unban); } }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="planProvider">Fournisseur de plan</param>
        public MainWindowViewModel(IPlanProvider planProvider)
        {
            this.planProvider = planProvider;
            nodeViewModels = new SortedDictionary<int, PlanNodeViewModel>();
        }

        /// <summary>
        /// Lance la reception du premier plan
        /// </summary>
        /// <returns>Task (methode asynchrone)</returns>
        public async void Init()
        {
            await LaunchPlanRetrievingAsync();
        }

        /// <summary>
        /// Lance la recuperation d'un plan aupres du provider
        /// </summary>
        /// <returns>Task (methode asynchrone)</returns>
        private async Task LaunchPlanRetrievingAsync()
        {
            this.plan = await planProvider.GetPlanAsync();
            UpdateMsaglGraphAndViewModelProperties();
        }

        /// <summary>
        /// Update la reprentation Msagl du plan courant et les view modeles des noeuds (champ 'Nodes')
        /// </summary>
        private void UpdateMsaglGraphAndViewModelProperties()
        {
            nodeViewModels.Clear();
            NodesNotInPlan = new ObservableCollection<PlanNodeViewModel>(plan.ManualEvents.Select(n => new PlanNodeViewModel(n.Value)));
            NodeSelectedInAddList = null;
            Criterias = new List<CriteriaViewModel>(plan.PlanCriterias.Select(c => new CriteriaViewModel(c.Key, c.Value)));
            Abilities = new List<AbilityViewModel>(plan.Abilities.Select(h => new AbilityViewModel(h.Key, h.Value)));
            SelectedCriteria = null;

            Graph mgraph = new Graph();
            if(plan != null)
            {
                foreach (PlanNode pNode in plan.Graph.Nodes.Values)
                {
                    //Creation du viewmodel gerant l'etat d'affichage du noeud
                    PlanNodeViewModel viewModel = new PlanNodeViewModel(pNode);
                    nodeViewModels.Add(viewModel.Id, viewModel);

                    Node n = mgraph.AddNode(pNode.Id.ToString());
                    var foreground = viewModel.ForegroundColor;
                    var background = viewModel.BackgroundColor;
                    n.Attr.Color = new Color((byte)(viewModel.Opacity * 255), foreground.R, foreground.G, foreground.B);
                    n.Attr.FillColor = new Color((byte)(viewModel.Opacity * 255), background.R, background.G, background.B);
                    n.UserData = viewModel;
                    n.Attr.LineWidth = 1.5;
                    foreach (var fils in pNode.Children)
                        mgraph.AddEdge(viewModel.Id.ToString(), fils.Key.ToString());
                }
            }

            MsaglGraph = mgraph;
        }
        
        /// <summary>
        /// Met a jour la liste des events pour mettre en avant ceux qui peuvent etre lies avec le noeud selectionne
        /// </summary>
        private void OnNodeSelected()
        {
            PlanNodeViewModel selectedNode = SelectedIdAsString == null ? null : nodeViewModels[int.Parse(SelectedIdAsString)];
            if(selectedNode != null && !selectedNode.IsActive)
            {
                NodeSelectedInAddList = null;
            }

            //Mise en avant des taches pouvant être liees au noeud selectionne
            foreach(PlanNodeViewModel manualEvent in NodesNotInPlan)
            {
                manualEvent.IsActive = selectedNode == null
                                       || manualEvent.Node.AllowedPredecessors.Contains(selectedNode.Id)
                                       || manualEvent.Node.AllowedPredecessors.Contains(selectedNode.Id);
            }

            UpdateAddButtonState();
            UpdateBanButtonsState();
        }

        /// <summary>
        /// Met a jour les view models des noeuds pour mettre en avant ceux qui peuvent etre lies avec l'event selectionne
        /// </summary>
        private void OnNodesNotInPlanSelected()
        {
            foreach (var viewModel in nodeViewModels)
            {
                if (nodeSelectedInAddList == null || nodeSelectedInAddList.Node.AllowedPredecessors.Contains(viewModel.Value.Id)
                                                  || nodeSelectedInAddList.Node.AllowedSuccessors.Contains(viewModel.Value.Id))
                    viewModel.Value.IsActive = true;
                else
                    viewModel.Value.IsActive = false;
            }

            UpdateAddButtonState();
        }

        /// <summary>
        /// Activation / Desactivation des boutons d'ajout de tache
        /// </summary>
        private void UpdateAddButtonState()
        {
            if (nodeSelectedInAddList == null || SelectedIdAsString == null)
            {
                AddBeforeButtonEnabled = false;
                AddAfterButtonEnabled = false;
            }
            else
            {
                AddBeforeButtonEnabled = nodeSelectedInAddList.Node.AllowedSuccessors.Contains(int.Parse(SelectedIdAsString));
                AddAfterButtonEnabled = nodeSelectedInAddList.Node.AllowedPredecessors.Contains(int.Parse(SelectedIdAsString));
            }
        }

        private void UpdateBanButtonsState()
        {
            int id;
            if(int.TryParse(SelectedIdAsString, out id) && id != 0)
            {
                if(nodeViewModels[id].IsBanned)
                {
                    BanVisibility = Visibility.Collapsed;
                    UnbanVisibility = Visibility.Visible;
                }
                else
                {
                    BanVisibility = Visibility.Visible;
                    UnbanVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                BanVisibility = Visibility.Collapsed;
                UnbanVisibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Met a jour les view models des noeuds avec le nom du critere selectionne
        /// </summary>
        private void OnCriteriaSelected()
        {
            foreach (var n in nodeViewModels)
                n.Value.SelectedCriteriaName = selectedCriteria == null ? null : selectedCriteria.Name;
        }

        /// <summary>
        /// Met a jour les view model des noeuds pour mettre en avant l'habilite selectionnee
        /// </summary>
        /// <param name="lastValue">L'habilite selectionnee precedemment</param>
        private void OnAbilitySelected(AbilityViewModel lastValue = null)
        {
            PlanNodeViewModel nodeViewModel;
            if (lastValue != null)
            {
                foreach (int id in lastValue.NodeIds)
                {
                    if (nodeViewModels.TryGetValue(id, out nodeViewModel))
                        nodeViewModel.BackgroundColor = PlanNodeViewModel.DefaultBackground;
                }
            }
            if(SelectedAbility != null)
            {
                foreach (int id in SelectedAbility.NodeIds)
                {
                    if (nodeViewModels.TryGetValue(id, out nodeViewModel))
                        nodeViewModels[id].BackgroundColor = PlanNodeViewModel.AbilitySelectedBackground;
                }
            }
        }

        /// <summary>
        /// Lance l'execution du plan affiche actuellement dans l'interface
        /// </summary>
        private void RunPlan()
        {
            PlanNodeViewModel startNode;
            if (!nodeViewModels.TryGetValue(0, out startNode) || startNode.GraphViewer == null)
                return;

            PlanRunner runner = new PlanRunner(startNode.GraphViewer, nodeViewModels, startNode);
            runner.Start();
        }

        /// <summary>
        /// Ajoute l'event selectionne comme pere du noeud selectionne
        /// </summary>
        private void AddBefore()
        {
            int childId;
            int newId = plan.Graph.Nodes.Min(n => n.Value.Id) - 1;
            if (!int.TryParse(SelectedIdAsString, out childId))
                return;
            // Creation et ajout du noeud
            PlanNode newNode = new PlanNode(newId);
            newNode.Label = NodeSelectedInAddList.Label;
            newNode.Duration = NodeSelectedInAddList.Node.Duration;
            newNode.Criterias = NodeSelectedInAddList.Node.Criterias;
            plan.Graph.Nodes.Add(newNode.Id, newNode);
            plan.Graph.CreateArc(childId, newNode.Id);

            UpdateMsaglGraphAndViewModelProperties();
        }

        /// <summary>
        /// Ajoute l'event selectionne comme fils du noeud selectionne
        /// </summary>
        private void AddAfter()
        {
            int fatherId;
            int newId = plan.Graph.Nodes.Min(n => n.Value.Id) - 1;
            if (!int.TryParse(SelectedIdAsString, out fatherId))
                return;
            // Creation et ajout du noeud
            PlanNode newNode = new PlanNode(newId);
            newNode.Label = NodeSelectedInAddList.Label;
            newNode.Duration = NodeSelectedInAddList.Node.Duration;
            newNode.Criterias = NodeSelectedInAddList.Node.Criterias;
            plan.Graph.Nodes.Add(newNode.Id, newNode);
            plan.Graph.CreateArc(fatherId, newNode.Id);

            UpdateMsaglGraphAndViewModelProperties();
        }

        /// <summary>
        /// Banni le noeud selectionne
        /// </summary>
        private void Ban()
        {
            int id;
            if (int.TryParse(SelectedIdAsString, out id))
                nodeViewModels[id].IsBanned = true;

            UpdateBanButtonsState();
        }

        /// <summary>
        /// Debanni le noeud selectionne
        /// </summary>
        private void Unban()
        {
            int id;
            if (int.TryParse(SelectedIdAsString, out id))
                nodeViewModels[id].IsBanned = false;

            UpdateBanButtonsState();
        }
    }
}
