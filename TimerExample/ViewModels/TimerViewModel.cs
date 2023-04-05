﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TimerExample.Models;

namespace TimerExample.ViewModels;

public class TimerViewModel : INotifyPropertyChanged
{
    private TimerModel _timerModel;
    private int _secondsLeft;
    private ICommand _startCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int SecondsLeft
    {
        get => _secondsLeft;
        set
        {
            _secondsLeft = value;
            OnPropertyChanged();
        }
    }

    public ICommand StartCommand
    {
        get
        {
            return _startCommand 
                   ?? (_startCommand = new RelayCommand(
                       param => Start(),
                       param => !_timerModel.IsRunning
                       ));
        }
    }

    public TimerViewModel()
    {
        _timerModel = new TimerModel();
        _timerModel.TimeChanged += OnTimeChanged;
    }

    public void Start() => _timerModel.Start();

    public void Stop() => _timerModel.Stop();

    private void OnTimeChanged(object sender, int secondsLeft)
    {
        SecondsLeft = secondsLeft;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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