using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ITVMusic.ViewModels {
    public class ViewModelCommand : ICommand {

        // Fields
        private readonly Action<object?> m_executeAction;
        private readonly Predicate<object?>? m_canExecuteAction;

        // Constructors
        public ViewModelCommand(Action<object?> executeAction, Predicate<object?>? canExecuteAction) {
            m_executeAction = executeAction;
            m_canExecuteAction = canExecuteAction;
        }

        public ViewModelCommand(Action<object?> executeAction)
            : this(executeAction, null) { }

        // Events
        public event EventHandler? CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) =>
            m_canExecuteAction is null || m_canExecuteAction(parameter);

        public void Execute(object? parameter) => m_executeAction(parameter);
    }
}
