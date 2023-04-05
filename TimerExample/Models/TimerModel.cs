using System;
using System.Timers;

namespace TimerExample.Models;

public class TimerModel
{
    private Timer _timer;
    private int _secondsLeft;

    public event EventHandler<int> TimeChanged;

    public bool IsRunning { get; private set; }
    public TimerModel()
    {
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimedEvent;
        _secondsLeft = 60;
    }

    public void Start()
    {
        IsRunning = true;
        _timer.Start();
    }

    public void Stop()
    {
        IsRunning = false;
        _timer.Stop();
    }

    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        _secondsLeft--;
        TimeChanged?.Invoke(this, _secondsLeft);

        if (_secondsLeft == 0) Stop();
    }
}