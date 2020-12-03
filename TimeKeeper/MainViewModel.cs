using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TimeKeeper.Annotations;

namespace TimeKeeper
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly IView view;
        private TimeSpan originalTimeSpan;
        private TimeSpan timeSpan;
        private readonly Timer timer;
        private string time;
        private bool isElapsed;

        public MainViewModel(IView view)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            timer = new Timer(500);
            timer.Elapsed += OnTimerElapsed;
            Time = "00:00";
            SetTimerToCommand = new AsyncCommand<string>(SetTimerTo);
            ToggleTimerCommand = new AsyncCommand(ToggleTimer);
            ResetTimerCommand = new AsyncCommand(ResetTimer);
            CloseCommand = new AsyncCommand(Close);
        }

        public string Time
        {
            get => time;
            set
            {
                if (value == time) return;
                time = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetTimerToCommand { get; }

        private Task SetTimerTo(string minutes)
        {
            originalTimeSpan = TimeSpan.FromMinutes(int.Parse(minutes));
            timeSpan = originalTimeSpan;
            timer.Start();
            return Task.CompletedTask;
        }
        
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            timeSpan -= TimeSpan.FromMilliseconds(timer.Interval);
            IsElapsed = timeSpan < TimeSpan.Zero;
            Time = timeSpan.ToFormattedTimeSpan();
        }

        public ICommand ToggleTimerCommand { get; }

        private Task ToggleTimer()
        {
            timer.Enabled = !timer.Enabled;
            return Task.CompletedTask;
        }

        public ICommand ResetTimerCommand { get; }

        private Task ResetTimer()
        {
            timeSpan = originalTimeSpan;
            return Task.CompletedTask;
        }

        public ICommand CloseCommand { get; }

        private Task Close()
        {
            view.Close();
            return Task.CompletedTask;
        }

        public bool IsElapsed
        {
            get => isElapsed;
            set
            {
                if (value == isElapsed) return;
                isElapsed = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
