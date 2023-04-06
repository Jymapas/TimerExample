using System;
using System.Timers;

namespace TimerExample.Models;

public class TimerModel
{
    private const int _totalTime = 60;
    private int _remainingTime;
    private Timer _timer;

    public TimerModel()
    {
        _remainingTime = _totalTime;
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
    }

    public event Action TimerTick;

    public int RemainingTime
    {
        get { return _remainingTime; }
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        Reset();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _remainingTime--;
        TimerTick?.Invoke();
        if (_remainingTime == 0)
        {
            _timer.Stop();
        }
    }

    private void Reset()
    {
        _remainingTime = _totalTime;
    }
}