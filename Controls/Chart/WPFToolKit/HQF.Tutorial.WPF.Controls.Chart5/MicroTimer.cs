using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HQF.Tutorial.WPF.Controls.Chart5
{
    public class MicroTimer
    {

        private readonly Queue<long> _TickTimeTable;
        private readonly Thread _Thread;
        private readonly long _MaxDelayInTicks;  // do not run if the delay was too long
        private long _NextWakeUpTickTime;

        public delegate void dOnMicroTimer(int xSenderThreadId, long xWakeUpTimeInTicks, long xDelayInTicks);
        public event dOnMicroTimer OnMicroTimer;
        public event dOnMicroTimer OnMicroTimerSkipped;

        public delegate void dQuickNote(int xSenderThreadId);
        public event dQuickNote OnMicroTimerStart;
        public event dQuickNote OnMicroTimerStop;

        public MicroTimer(Queue<long> xTickTimeTable, long xMaxDelayInTicks)
        {
            _TickTimeTable = xTickTimeTable;
            _Thread = new Thread(new ThreadStart(Loop));
            _Thread.Priority = ThreadPriority.Highest;
            _Thread.Name = "TimerLoop";
            _Thread.IsBackground = true;
            _MaxDelayInTicks = xMaxDelayInTicks;
        } //

        public int Start()
        {
            if ((_Thread.ThreadState & System.Threading.ThreadState.Unstarted) == 0) return -1;
            _Thread.Start();
            return _Thread.ManagedThreadId;
        } //

        public void Stop()
        {
            _Thread.Interrupt();
        } //

        private void Loop()
        {
            dQuickNote lOnStart = OnMicroTimerStart;
            if (lOnStart != null) lOnStart(_Thread.ManagedThreadId);

            try
            {
                while (true)
                {
                    if (_TickTimeTable.Count < 1) break;
                    _NextWakeUpTickTime = _TickTimeTable.Dequeue();
                    long lMilliseconds = _NextWakeUpTickTime - Stopwatch.GetTimestamp();
                    if (lMilliseconds < 0L) continue;
                    lMilliseconds = (lMilliseconds * 1000) / Stopwatch.Frequency;
                    lMilliseconds -= 50;  // we want to wake up earlier and spend the last time using SpinWait
                    Thread.Sleep((int)lMilliseconds);

                    while (Stopwatch.GetTimestamp() < _NextWakeUpTickTime)
                    {
                        Thread.SpinWait(10);
                    }
                    long lWakeUpTimeInTicks = Stopwatch.GetTimestamp();
                    long lDelay = lWakeUpTimeInTicks - _NextWakeUpTickTime;
                    if (lDelay < _MaxDelayInTicks)
                    {
                        dOnMicroTimer lHandler = OnMicroTimer;
                        if (lHandler == null) continue;
                        lHandler(_Thread.ManagedThreadId, lWakeUpTimeInTicks, lDelay);
                    }
                    else
                    {
                        dOnMicroTimer lHandler = OnMicroTimerSkipped;
                        if (lHandler == null) continue;
                        lHandler(_Thread.ManagedThreadId, lWakeUpTimeInTicks, lDelay);
                    }
                }
            }
            catch (ThreadInterruptedException) { }
            catch (Exception) { Console.WriteLine("Exiting timer thread."); }

            dQuickNote lOnStop = OnMicroTimerStop;
            if (lOnStop != null) lOnStop(_Thread.ManagedThreadId);
        } //

        public static double getDateTimeToStopwatchTickRatio()
        {
            const double cMeasurements = 100.0;
            double lTotalMilliseconds = 0.0;
            double lTotalTicks = 0.0;

            // averaging to interpolate the inprecision of the DateTime class
            // note: DateTime ticks are not Stopwatch ticks!
            for (double i = 0; i < cMeasurements + 0.001; i++)
            {
                DateTime lDateTime = DateTime.Now;
                long lTicks = Stopwatch.GetTimestamp();
                TimeSpan lTimeSpan = lDateTime.TimeOfDay;
                lTotalMilliseconds += lTimeSpan.TotalMilliseconds / cMeasurements;
                lTotalTicks += lTicks / cMeasurements;
                Thread.Sleep(5);
            }

            return lTotalTicks / lTotalMilliseconds;
        } //

        public static long convertTimeToTicks(DateTime xDateTime, double xRatio)
        {
            return (long)(xDateTime.TimeOfDay.TotalMilliseconds * xRatio);
        } //

        public static DateTime convertTicksToTime(long xTicks, double xRatio)
        {
            DateTime lToday = DateTime.Today;
            lToday = lToday.AddMilliseconds(xTicks / xRatio);
            return lToday;
        } //

    } // class
}
