﻿using GalaSoft.MvvmLight;
using System;
using PrimeFinder.Model;

namespace PrimeFinder.ViewModel
{
    /// <summary>
    /// The PrimeFinderViewModel, using the MVVM Light framework.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public sealed class PrimeFinderViewModel : ViewModelBase, IDisposable
    {
        private PrimeFinderModel PrimeFinder;
        private PrimeTimerModel PrimeTimer { get; set; }

        // Private backing fields
        private string _windowTitle = string.Empty;
        private int _maxPrime = 0;
        private string _statusMessage = string.Empty;
        private int _counter = 0;

        /// <summary>
        /// Constructor for PrimeFinderViewModel
        /// </summary>
        public PrimeFinderViewModel(IDataService dataService)
        {
            WindowTitle = "Prime Number Generator";

            StatusMessage = "Running";

            // Start the timer
            PrimeTimer = new PrimeTimerModel(60);

            // Listen for timer tick events
            PrimeTimer.DispatcherTimer.Tick += new EventHandler(OnTick);

            // Listen for the timer completed event
            PrimeTimer.TimerCompleted += new EventHandler(OnTimerCompleted);

            // Start the prime finder            
            PrimeFinder = new PrimeFinderModel(100000000);
            PrimeFinder.Start();

            // Listen for the PrimeFound event
            PrimeFinder.PrimeFound += new EventHandler(OnPrimeFound);
        }

        /// <summary>
        /// Window title.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return _windowTitle;
            }
            set
            {
                Set(ref _windowTitle, value);
            }
        }

        /// <summary>
        /// The maximum prime found
        /// </summary>
        public int MaxPrime
        {
            get
            {
                return _maxPrime;
            }
            set
            {
                Set(ref _maxPrime, value);
            }
        }

        /// <summary>
        /// The timer counter.
        /// </summary>
        public int Counter
        {
            get
            {
                return _counter;
            }
            set
            {
                Set(ref _counter, value);
            }
        }

        /// <summary>
        /// Shows running status for tool.
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                Set(ref _statusMessage, value);
            }
        }

        /// <summary>
        /// Update the counter for every timer tick
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        //private void OnTick(object sender, EventArgs e)
        private void OnTick(object sender, EventArgs e)
        {
            Counter = PrimeTimer.Counter;
        }

        /// <summary>
        /// When the timer stops, stop the prime finder and update the status message.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void OnTimerCompleted(object sender, EventArgs e)
        {
            StatusMessage = "Max time elapsed";
            PrimeFinder.StopPrimeFinder();
        }

        /// <summary>
        /// Whenever a prime is found, update MaxPrime
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void OnPrimeFound(object sender, EventArgs e)
        {
            MaxPrime = PrimeFinder.MaxPrime;            
        }

        public void Dispose()
        {
            PrimeFinder.Dispose();
        }
    }
}