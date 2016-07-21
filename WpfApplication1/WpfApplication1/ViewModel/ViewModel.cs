using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApplication1.Command;

namespace WpfApplication1.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private int _timesClicked;
        private int _delayedTimesClicked;

        public ViewModel()
        {
            InitCommand();
        }

        public ICommand ClickCommand { get; set; }
        public ICommand DelayedClickCommand { get; set; }

        private void InitCommand()
        {
            ClickCommand = new DelayedCommand(ExecuteClickCommand);
            DelayedClickCommand = new DelayedCommand(ExecuteDelayedClickCommand, TimeSpan.FromMilliseconds(200));
        }

        private void ExecuteDelayedClickCommand()
        {
            DelayedTimesClicked = ((DelayedCommand)DelayedClickCommand).TimesClicked;
        }

        private void ExecuteClickCommand()
        {
            TimesClicked = ((DelayedCommand)ClickCommand).TimesClicked;
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

        public int DelayedTimesClicked
        {
            get { return _delayedTimesClicked; }
            set
            {
                _delayedTimesClicked = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
