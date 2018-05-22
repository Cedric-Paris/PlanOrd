using Microsoft.Msagl.Drawing;
using PlanOrd.Model;
using PlanOrd.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PlanOrd.ViewModel
{
    public class MainWindowViewModel : ViewModel
    {
        private IPlanProvider planProvider;
        private Plan plan;
        private Graph msaglGraph;
        private string selectedIdAsString;
        private IDictionary<int, PlanNodeViewModel> nodeViewModels;
        private ObservableCollection<PlanNodeViewModel> nodesNotInPlan;
        private PlanNodeViewModel nodeSelectedInAddList;
        private IList<CriteriaViewModel> criterias;
        private CriteriaViewModel selectedCriteria;

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
                foreach (var n in nodeViewModels)
                    n.Value.SelectedCriteriaName = selectedCriteria == null ? null : selectedCriteria.Name;
                OnPropertyChanged("SelectedCriteria");
            }
        }

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
        /// Lance la recuperation d'un plan aupres du provideur
        /// </summary>
        /// <returns>Task (methode asynchrone)</returns>
        public async Task LaunchPlanRetrievingAsync()
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
            NodesNotInPlan = new ObservableCollection<PlanNodeViewModel>(plan.EventsNotInPlan.Select(n => new PlanNodeViewModel(n.Value)));
            NodeSelectedInAddList = null;
            Criterias = new List<CriteriaViewModel>(plan.Graph.Nodes[2].Criterias.Select(c=>new CriteriaViewModel(c.Key, c.Value)));
            SelectedCriteria = null;

            Graph mgraph = new Graph();
            if(plan != null)
            {
                foreach (PlanNode pNode in plan.Graph.Nodes.Values)
                {
                    //Creation du viewmodel gerant l'etat d'affichage du noeud
                    PlanNodeViewModel viewModel = new PlanNodeViewModel(pNode);
                    nodeViewModels.Add(viewModel.Id, viewModel);

                    mgraph.AddNode(pNode.Id.ToString()).UserData = viewModel;
                    foreach (var fils in pNode.Children)
                        mgraph.AddEdge(viewModel.Id.ToString(), fils.Key.ToString());
                }
            }

            MsaglGraph = mgraph;
        }
    }
}
