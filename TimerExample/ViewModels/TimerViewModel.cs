using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TimerExample.Models;

namespace TimerExample.ViewModels;

public class TimerViewModel : INotifyPropertyChanged
{
    private TimerModel _model;
    private bool _isRunning;

    public TimerViewModel()
    {
        _model = new TimerModel();
        _model.TimerTick += OnTimerTick;
        StartCommand = new RelayCommand(StartTimer);
        StopCommand = new RelayCommand(StopTimer);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public int RemainingTime
    {
        get { return _model.RemainingTime; }
    }

    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            if (_isRunning != value)
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }
    }

    public ICommand StartCommand { get; private set; }

    public ICommand StopCommand { get; private set; }

    private void OnTimerTick()
    {
        OnPropertyChanged(nameof(RemainingTime));
    }

    private void StartTimer(object parameter)
    {
        if (!IsRunning)
        {
            _model.Start();
            IsRunning = true;
        }
    }

    private void StopTimer(object parameter)
    {
        if (IsRunning)
        {
            _model.Stop();
            IsRunning = false;
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
    
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}