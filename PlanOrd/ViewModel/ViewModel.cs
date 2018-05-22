using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlanOrd.ViewModel
{
    /// <summary>
    /// Classe de base pour les view modeles
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
