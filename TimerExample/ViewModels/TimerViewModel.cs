using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TimerExample.Models;

namespace TimerExample.ViewModels
{
    public sealed class TimerViewModel : INotifyPropertyChanged
    {
        private readonly TimerModel _timerModel;
        private int _secondsLeft;
        private int _displayTime = 60;
        const int CountdownSeconds = 10;
        private Mode _mode;
        
        enum Mode
        {
            Question,
            Doublet,
            Blitz,
            Countdown,
            None
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int SecondsLeft
        {
            set
            {
                _secondsLeft = value;
                OnPropertyChanged();
            }
        }

        public int DisplayTime
        {
            get => _displayTime;
            private set
            {
                _displayTime = value; 
                OnPropertyChanged();
            }
        }

        public ICommand StartTimerCommand { get; }
        public ICommand StopTimerCommand { get; }
        public ICommand StartTimer30Command { get; }
        public ICommand StartTimer20Command { get; }

        public TimerViewModel()
        {
            _mode = Mode.None;
            
            _timerModel = new TimerModel();
            _timerModel.TimeChanged += OnTimeChanged;

            StartTimerCommand = new RelayCommand(() => StartTimer(60, Mode.Question));
            StopTimerCommand = new RelayCommand(StopTimer);
            StartTimer30Command = new RelayCommand(() => StartTimer(30, Mode.Doublet));
            StartTimer20Command = new RelayCommand(() => StartTimer(20, Mode.Blitz));
        }

        private void StartTimer(int seconds, Mode currentMode)
        {
            _mode = currentMode;
            
            SecondsLeft = seconds;
            _timerModel.Start(seconds);
        }

        private void StopTimer()
        {
            _mode = Mode.None;
            
            _timerModel.Stop();
            SecondsLeft = DisplayTime = 60;
        }

        private void FinalCountdown()
        {
            _mode = Mode.Countdown;
            SecondsLeft = DisplayTime = CountdownSeconds;
            _timerModel.Start(CountdownSeconds);
        }

        private void OnTimeChanged(object sender, int secondsLeft)
        {
            SecondsLeft = secondsLeft;
            DisplayTime--;
            
            if(secondsLeft == 0)
                _timerModel.Stop();
            if (DisplayTime != 0) return;
            if(_mode == Mode.Countdown) 
                StopTimer();
            else
                FinalCountdown();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}