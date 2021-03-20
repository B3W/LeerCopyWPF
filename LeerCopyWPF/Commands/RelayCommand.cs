using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LeerCopyWPF.Commands
{
    /// <summary>
    /// Non-generic ICommand implementation that ensures WPF queries ready
    /// status of RelayCommands when they query ready status of built-in
    /// commands by using CommandManager.RequerySuggested.
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


        #region Properties

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion // ICommand Members

        #endregion // Properties


        #region Methods

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute", "Command's Action cannot be null");
            _canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members

        #endregion // Methods
    }

    /// <summary>
    /// Generic ICommand implementation that ensures WPF queries ready
    /// status of RelayCommands when they query ready status of built-in
    /// commands by using CommandManager.RequerySuggested.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        /// <summary>
        /// Logic to run when command executed
        /// </summary>
        private readonly Action<T> _execute;

        /// <summary>
        /// Logic for determining if command is able to be executed
        /// </summary>
        private readonly Predicate<object> _canExecute;

        #endregion // Fields


        #region Properties

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion // ICommand Members

        #endregion // Properties


        #region Methods

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute", "Command's Action cannot be null");
            _canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            T castParameter = (T)Convert.ChangeType(parameter, typeof(T));
            _execute(castParameter);
        }

        #endregion // ICommand Members

        #endregion // Methods
    }
}
