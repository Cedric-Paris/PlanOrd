using System;
using System.Windows.Input;

namespace PlanOrd.ViewModel
{
    public class ButtonCommand : ICommand
    {
        private Action actionOnExecute;
        public event EventHandler CanExecuteChanged;

        public ButtonCommand(Action actionOnExecute)
        {
            this.actionOnExecute = actionOnExecute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            actionOnExecute();
        }
    }
}
