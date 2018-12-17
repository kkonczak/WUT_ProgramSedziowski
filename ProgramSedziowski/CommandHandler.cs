using System;
using System.Windows.Input;

namespace ProgramSedziowski
{
    public class CommandHandler : ICommand
    {
        private Action _command;

        public CommandHandler(Action command)
        {
            _command = command;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _command.Invoke();
        }
    }
}
