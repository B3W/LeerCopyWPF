using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LeerCopyWPF.Commands
{
    /// <summary>
    /// ICommand implementation that ensures WPF queries ready status of
    /// RelayCommands when they query ready status of built-in commands by
    /// using CommandManager.RequerySuggested.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields
        /// <summary>
        /// Logic to run when command executed
        /// </summary>
        private readonly Action<object> _execute;
        /// <summary>
        /// Logic for determining if command is able to be executed
        /// </summary>
        private readonly Predicate<object> _canExecute;
        #endregion // Fields

        #region Constructors
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute", "Command's Action cannot be null");
            }
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion // ICommand Members
    }
}
