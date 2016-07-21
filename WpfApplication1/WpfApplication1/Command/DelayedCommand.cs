using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApplication1.Command
{
    public class DelayedCommand : ICommand
    {
        private readonly Action _methodToExecute;
        private readonly Func<bool> _canExecuteEvaluator;
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly TimeSpan _delayTime;

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
        public DelayedCommand(Action methodToExecute, Func<bool> canExecuteEvaluator, TimeSpan delayTime)
        {
            _methodToExecute = methodToExecute;
            _canExecuteEvaluator = canExecuteEvaluator;
            _delayTime = delayTime;
            
            _dispatcherTimer = new DispatcherTimer(delayTime, DispatcherPriority.Normal, Callback, Application.Current.Dispatcher);
        }

        /// <summary>
        /// A command to stop the spamming of the <see cref="Execute"/> method
        /// when no <see cref="CanExecute"/> method is required
        /// </summary>
        /// <param name="methodToExecute">Method to run when command executes</param>
        /// <param name="delayTime">The cool down period required between click execution</param>
        public DelayedCommand(Action methodToExecute, TimeSpan delayTime)
            : this(methodToExecute, null, delayTime)
        {
        }

        /// <summary>
        /// A command when only a <see cref="Execute"/> method is needed
        /// </summary>
        /// <param name="methodToExecute">Method to run when command executes</param>
        public DelayedCommand(Action methodToExecute)
            : this(methodToExecute, null, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// A command taking a <see cref="Execute"/> Method and a <see cref="CanExecute"/> method
        /// </summary>
        /// <param name="methodToExecute">Method to run when command executes</param>
        /// <param name="canExecuteEvaluator">Method used to determine if the command can execute</param>
        public DelayedCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
            : this(methodToExecute, canExecuteEvaluator, TimeSpan.Zero)
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

        public void Execute(object parameter)
        {
            if (!_dispatcherTimer.IsEnabled)
                TimesClicked = 0;

            TimesClicked++;

            _dispatcherTimer?.Stop();
            _dispatcherTimer?.Start();
        }

        private void Callback(object sender, EventArgs eventArgs)
        {
            _dispatcherTimer.Stop();
            _methodToExecute.Invoke();
        }
    }
}
