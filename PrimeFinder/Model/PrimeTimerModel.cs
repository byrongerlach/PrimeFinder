using System;
using System.Windows.Threading;

namespace PrimeFinder.Model
{
    class PrimeTimerModel
    {
        /// <summary>
        /// The maximum time the timer will run
        /// </summary>
        private int MaxSeconds { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxSeconds"></param>
        public PrimeTimerModel(int maxSeconds)
        {
            MaxSeconds = maxSeconds;
            Counter = 0;
            StartDispatcherTimer();
        }

        /// <summary>
        /// Dispatcher timer 
        /// </summary>
        public DispatcherTimer DispatcherTimer { get; set; }

        /// <summary>
        /// The tick counter for the timer
        /// </summary>
        public int Counter { get; set; }

        // The event handler for when the count maximum is reached.
        public event EventHandler TimerCompleted;

        /// <summary>
        /// Start the timer
        /// </summary>
        private void StartDispatcherTimer()
        {
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            DispatcherTimer.Tick += new EventHandler(OnTick);
            DispatcherTimer.Start();
        }

        /// <summary>
        /// Event handler for tick events
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void OnTick(object sender, EventArgs e)
        {
            Counter++;

            if (Counter == MaxSeconds)
            {
                DispatcherTimer.Stop();
                TimerCompleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}