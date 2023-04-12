using System;
using System.Timers;

namespace TimerExample.Models
{
    public class TimerModel
    {
        private Timer _timer;
        private int _secondsLeft;

        public event EventHandler<int> TimeChanged;

        public TimerModel()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimedEvent;
        }

        public void Start(int secondsLeft)
        {
            _secondsLeft = secondsLeft;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _secondsLeft--;
            TimeChanged?.Invoke(this, _secondsLeft);

            if (_secondsLeft == 0)
            {
                _timer.Stop();
            }
        }
    }
}