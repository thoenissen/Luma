using System;
using System.Windows.Input;

namespace Seth.Luma.Core.Behaviors
{
    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    /// <typeparam name="T">Type of parameter</typeparam>
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        /// <summary>
        /// Defines the method to be called when the command is invoked
        /// </summary>
        private readonly Action<T> _execute;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state
        /// </summary>
        private readonly Predicate<T> _canExecute;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Defines the method to be called when the command is invoked</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Defines the method to be called when the command is invoked</param>
        /// <param name="canExecute">Defines the method that determines whether the command can execute in its current state</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion // Constructor

        #region ICommand Member

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <param name="parameter">Parameter</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(Object parameter)
        {
            return parameter is T && (_canExecute == null || _canExecute((T)parameter));
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="parameter">Parameter</param>
        public void Execute(object parameter)
        {
            if (CanExecute((T)parameter))
            {
                _execute((T)parameter);
            }
        }

        #endregion // ICommand Member
    }

    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// Defines the method to be called when the command is invoked
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state
        /// </summary>
        private readonly Func<bool> _canExecute;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Defines the method to be called when the command is invoked</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Defines the method to be called when the command is invoked</param>
        /// <param name="canExecute">Defines the method that determines whether the command can execute in its current state</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion // Constructor

        #region ICommand Member

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <param name="parameter">Parameter</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(Object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="parameter">Parameter</param>
        public void Execute(Object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        #endregion // ICommand Member
    }
}
