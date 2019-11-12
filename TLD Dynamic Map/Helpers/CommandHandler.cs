using System;
using System.Windows.Input;

namespace TLD_Dynamic_Map.Helpers.Helpers
{
    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action)
        {
            _action = action;
            _canExecute = true;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        #pragma warning disable
        public event EventHandler CanExecuteChanged;
        #pragma warning enable

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
