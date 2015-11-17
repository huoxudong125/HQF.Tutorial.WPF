using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HQF.Tutorial.WPF.Controls.Chart5
{
    public class Model
    {
        private const int cNumTasks = 20;
        private const int cNumTriggers = 41; // we discard the first measurement
        private ViewModel _ViewModel;
        private double _DateTimeToStopwatchTickRatio;

        public Model(ViewModel xViewModel)
        {
            _DateTimeToStopwatchTickRatio = MicroTimer.getDateTimeToStopwatchTickRatio();
            _ViewModel = xViewModel;
            StartTimer(cNumTriggers);
        } // constructor

        private void StartTimer(int xNumTriggers)
        {
            DateTime lDateTime = DateTime.Now.AddSeconds(3.0); // an arbitrary ideal start time

            long lStartTimeInStopwatchTicks = MicroTimer.convertTimeToTicks(lDateTime, _DateTimeToStopwatchTickRatio);
            double lStartInXSeconds = (lStartTimeInStopwatchTicks - Stopwatch.GetTimestamp()) / Stopwatch.Frequency;
            //_ViewModel.InfoBoxText =  lStartInXSeconds + " secs" ;

            long[] lSchedule = new long[xNumTriggers];
            long lTwoSecsInTicks = 2 * Stopwatch.Frequency;
            for (int i = 0; i < xNumTriggers; i++)
            {
                lSchedule[i] = lStartTimeInStopwatchTicks;
                lStartTimeInStopwatchTicks += lTwoSecsInTicks;
            }

            long lMaxDelay = (5L * Stopwatch.Frequency) / 1000L; // 5 ms
            MicroTimer lMicroTimer = new MicroTimer(new Queue<long>(lSchedule), lMaxDelay);
            lMicroTimer.OnMicroTimer += OnMicroTimer;
            lMicroTimer.OnMicroTimerStop += OnMicroTimerStop;
            lMicroTimer.OnMicroTimerSkipped += OnMicroTimerSkipped;
            lMicroTimer.Start();
        } //

        private void OnMicroTimerSkipped(int xSenderThreadId, long xWakeUpTimeInTicks, long xDelayInTicks)
        {
            _ViewModel.AddInfoText("OnMicroTimerSkipped event raised");
        } //

        private void OnMicroTimerStop(int xSenderThreadId)
        {
            _ViewModel.AddInfoText("OnMicroTimerStop event raised");
        } //

        private double _NumberOfMicroTimerCalls;
        private bool _Once = true;
        private void OnMicroTimer(int xSenderThreadId, long xWakeUpTimeInTicks, long xDelayInTicks)
        {
            _ViewModel.AddInfoText("OnMicroTimer event raised");
            CurvePoint[][] lPoints = new CurvePoint[cNumTasks][];
            ParallelLoopResult lResult = Parallel.For(0, cNumTasks, (xInt) => DoSomething(xInt, xWakeUpTimeInTicks, lPoints));

            // the first measurement is not precise, so we discard it
            if (_Once)
            {
                _Once = false;
                return;
            }

            double lCallNo = _NumberOfMicroTimerCalls++;
            lCallNo /= cNumTriggers;

            _ViewModel.AddNewCurves(lPoints, lCallNo);
        } //

        private void DoSomething(int xId, long xWakeUpTimeInTicks, CurvePoint[][] xPoints)
        {
            // time sensitive stuff first
            double lFromTicks = Stopwatch.GetTimestamp();
            Thread.Sleep(10);
            double lToTicks = Stopwatch.GetTimestamp();

            // time insensitive stuff
            double lFromMillisecs = (lFromTicks - xWakeUpTimeInTicks) / Stopwatch.Frequency * 1000.0;
            double lToMillisecs = (lToTicks - xWakeUpTimeInTicks) / Stopwatch.Frequency * 1000.0;
            CurvePoint lFrom = new CurvePoint(xId, lFromMillisecs);
            CurvePoint lTo = new CurvePoint(xId, lToMillisecs);
            xPoints[xId] = new CurvePoint[] { lFrom, lTo };
        } //

    } // class
}
