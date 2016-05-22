using System;
using System.Windows.Input;

namespace EntertainingExplosion.UserInterface
{
    public class CommandHandler : ICommand
    {
        private readonly Action action;
        private readonly bool canExecute;

        public CommandHandler(Action action, bool canExecute = true)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }
    }
}
