﻿using System;
using System.Windows.Input;
using WpfApplication1.Command;

namespace WpfApplication1.ViewModel
{
    public class ViewModel : BaseViewModel
    {
        private int _timesClicked;
        private int _delayedTimesClicked;
        private int _rateLimitTimesClicked;

        public ViewModel()
        {
            InitCommand();
        }

        public ICommand ClickCommand { get; set; }
        public ICommand DelayedClickCommand { get; set; }
        public ICommand RateLimitClickCommand { get; set; }

        private void InitCommand()
        {
            ClickCommand = new DelayedCommand(ExecuteClickCommand);
            DelayedClickCommand = new DelayedCommandTakingFunction(ExecuteDelayedClickCommand,
                TimeSpan.FromMilliseconds(200),
                (parameter, newParameter) => (int) parameter + (int) newParameter);

            RateLimitClickCommand = new RateLimitCommand(ExecuteRateLimitClickCommand, TimeSpan.FromSeconds(1));
        }

        private void ExecuteRateLimitClickCommand()
        {
            RateLimitTimesClicked = ((RateLimitCommand) RateLimitClickCommand).TimesClicked;
        }

        private void ExecuteDelayedClickCommand(object sender)
        {
            DelayedTimesClicked = (int) sender;
        }

        private void ExecuteClickCommand()
        {
            TimesClicked = ((DelayedCommand) ClickCommand).TimesClicked;
        }

        public int TimesClicked
        {
            get { return _timesClicked; }
            set
            {
                _timesClicked = value;
                OnPropertyChanged();
            }
        }

        public int TimesIncrease => 1;

        public int DelayedTimesClicked
        {
            get { return _delayedTimesClicked; }
            set
            {
                _delayedTimesClicked = value;
                OnPropertyChanged();
            }
        }

        public int RateLimitTimesClicked
        {
            get { return _rateLimitTimesClicked; }
            set
            {
                _rateLimitTimesClicked = value;
                OnPropertyChanged();
            }
        }
    }
}
