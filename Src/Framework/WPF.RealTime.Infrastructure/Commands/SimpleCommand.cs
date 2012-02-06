using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using WPF.RealTime.Infrastructure.Messaging;

namespace WPF.RealTime.Infrastructure.Commands
{
    /// <summary>
    /// Interface that is used for ICommands that notify when they are
    /// completed
    /// </summary>
    public interface ICompletionAwareCommand
    {
        /// <summary>
        /// Notifies that the command has completed
        /// </summary>
        //event Action<Object> CommandCompleted;

        WeakActionEvent<object> CommandCompleted { get; set; }
    }

    public class SimpleCommand<T1, T2> : ICommand, ICompletionAwareCommand
    {
        private readonly Func<T1, bool> _canExecuteMethod;
        private readonly Action<T2> _executeMethod;

        public SimpleCommand(Func<T1, bool> canExecuteMethod, Action<T2> executeMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            CommandCompleted = new WeakActionEvent<object>();
        }

        public SimpleCommand(Action<T2> executeMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = x => { return true; };
            CommandCompleted = new WeakActionEvent<object>();
        }

        public WeakActionEvent<object> CommandCompleted { get; set; }

        public bool CanExecute(T1 parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        public void Execute(T2 parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }

            //now raise CommandCompleted for this ICommand
            WeakActionEvent<object> completedHandler = CommandCompleted;
            if (completedHandler != null)
            {

                completedHandler.Invoke(parameter);
            }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T1)parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T2)parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "The this keyword is used in the Silverlight version")]
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class SimpleCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecuteMethod;
        private readonly Action<T> _executeMethod;

        public SimpleCommand(Func<T, bool> canExecuteMethod, Action<T> executeMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public SimpleCommand(Action<T> executeMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = x => true;
        }


        public bool CanExecute(T parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        public void Execute(T parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
    }
}
