using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApplication1.Command
{
    public class DelayedCommandTakingFunction : ICommand
    {
        private readonly Action<object> _methodToExecute;
        private readonly Func<bool> _canExecuteEvaluator;
        private readonly DispatcherTimer _dispatcherTimer;

        private readonly ParameterAggregate _parameterAggregate;
        public delegate object ParameterAggregate(object currentParameter, object newParameter);
        
        public int TimesClicked;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        

        /// <summary>
        /// A command to stop the spamming of the <see cref="Execute"/> method
        /// </summary>
        /// <param name="methodToExecute">Method to run when command executes</param>
        /// <param name="canExecuteEvaluator">Method used to determine if the command can execute</param>
        /// <param name="delayTime">The cool down period required between click execution</param>
        public DelayedCommandTakingFunction(Action<object> methodToExecute, Func<bool> canExecuteEvaluator, TimeSpan delayTime, ParameterAggregate parameterAggregate)
        {
            _methodToExecute = methodToExecute;
            _canExecuteEvaluator = canExecuteEvaluator;
            _parameterAggregate = parameterAggregate;
            
            _dispatcherTimer = new DispatcherTimer(delayTime, DispatcherPriority.Normal, Callback, Application.Current.Dispatcher);
            _dispatcherTimer.IsEnabled = false;
        }

        /// <summary>
        /// A command to stop the spamming of the <see cref="Execute"/> method
        /// when no <see cref="CanExecute"/> method is required
        /// </summary>
        /// <param name="methodToExecute">Method to run when command executes</param>
        /// <param name="delayTime">The cool down period required between click execution</param>
        public DelayedCommandTakingFunction(Action<object> methodToExecute, TimeSpan delayTime, ParameterAggregate parameterAggregate)
            : this(methodToExecute, null, delayTime, parameterAggregate)
        {
        }

        /// <summary>
        /// A command when only a <see cref="Execute"/> method is needed
        /// </summary>
        /// <param name="methodToExecute">Method to run when command executes</param>
        public DelayedCommandTakingFunction(Action<object> methodToExecute)
            : this(methodToExecute, null, TimeSpan.Zero, null)
        {
        }

        /// <summary>
        /// A command taking a <see cref="Execute"/> Method and a <see cref="CanExecute"/> method
        /// </summary>
        /// <param name="methodToExecute">Method to run when command executes</param>
        /// <param name="canExecuteEvaluator">Method used to determine if the command can execute</param>
        public DelayedCommandTakingFunction(Action<object> methodToExecute, Func<bool> canExecuteEvaluator)
            : this(methodToExecute, canExecuteEvaluator, TimeSpan.Zero, null)
        {
        }
        
        public bool CanExecute(object parameter)
        {
            if (_canExecuteEvaluator == null)
            {
                return true;
            }
            
            return _canExecuteEvaluator.Invoke();
        }

        private object _currentParameter;

        public void Execute(object parameter)
        {
            if (!_dispatcherTimer.IsEnabled)
                TimesClicked = 0;

            if (_currentParameter != null && _parameterAggregate != null)
                parameter = _parameterAggregate(_currentParameter, parameter);

            _currentParameter = parameter;

            _dispatcherTimer?.Stop();
            _dispatcherTimer?.Start();
        }

        private void Callback(object sender, EventArgs eventArgs)
        {
            _dispatcherTimer.Stop();
            _methodToExecute.Invoke(_currentParameter);

            _currentParameter = null;
        }
    }
}
