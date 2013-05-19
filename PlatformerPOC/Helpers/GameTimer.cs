using System;

namespace PlatformerPOC.Helpers
{
    public class GameTimer
    {
        private long stopwatchStart;

        public GameTimer()
        {
            Reset();
        }

        public void Reset()
        {
            stopwatchStart = TimeGetTime();
        }

        public bool Stopwatch(int ms)
        {
            if (TimeGetTime() > stopwatchStart + ms)
            {
                Reset();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Convert ticks to milliseconds. 10,000 ticks in 1 millisecond.
        /// </summary>        
        private long TimeGetTime()
        {
            return DateTime.Now.Ticks / 10000;
        }
    }

}