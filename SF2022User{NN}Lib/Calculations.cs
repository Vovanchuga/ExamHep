using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        public string[] AvailablePeriods(
            TimeSpan[] startTimes,
             int[] durations,
             TimeSpan beginWorkingTime,
             TimeSpan endWorkingTime,
             int consultationTime
             )
        {
            int TimeRangeCount = 0;
            string FreeTimeRange = "";
            bool skipTime = false;
            while (beginWorkingTime < endWorkingTime)
            {
                foreach (var time in startTimes)
                {
                    int TimeIndex = Array.IndexOf(startTimes, time);
                    TimeSpan nextTime = beginWorkingTime.Add(TimeSpan.FromMinutes(30));
                    if (durations[TimeIndex] < consultationTime)
                        durations[TimeIndex] += consultationTime;
                    if (nextTime > startTimes[startTimes.Length - 1] && nextTime < endWorkingTime)
                    {
                        beginWorkingTime = beginWorkingTime.Add(TimeSpan.FromMinutes(durations[durations.Length - 1] + nextTime.Minutes));
                        skipTime = true;
                        break;
                    }
                    if (beginWorkingTime == time)
                    {
                        if (nextTime > time)
                        {

                        }
                        beginWorkingTime = beginWorkingTime.Add(TimeSpan.FromMinutes(durations[TimeIndex]));
                        skipTime = true;
                        break;
                    }
                    else
                    {
                        skipTime = false;
                    }
                }
                if (skipTime == true)
                    continue;






                string Hourszero = "";
                string Minuteszero = "";

                if (beginWorkingTime.Hours < 10)   // замена часов с * на 0*
                    Hourszero = "0";
                if (beginWorkingTime.Minutes < 10) // замена минут с * на 0*
                    Minuteszero = "0";

                FreeTimeRange += $"{Hourszero}{beginWorkingTime.Hours}:{Minuteszero}{beginWorkingTime.Minutes}";
                beginWorkingTime = beginWorkingTime.Add(TimeSpan.FromMinutes(30));

                Hourszero = "";
                Minuteszero = "";

                if (beginWorkingTime.Hours < 10) // замена часов с * на 0*
                    Hourszero = "0";
                if (beginWorkingTime.Minutes < 10) // замена минут с * на 0*
                    Minuteszero = "0";
                if (beginWorkingTime > endWorkingTime)
                    beginWorkingTime = endWorkingTime;

                FreeTimeRange += $" - {Hourszero}{beginWorkingTime.Hours}:{Minuteszero}{beginWorkingTime.Minutes},";
            }
            foreach (var range in FreeTimeRange)
            {
                if (range == ',')
                    TimeRangeCount++;
            }

            string[] WorkGraph = new string[TimeRangeCount];
            string TimeRange = "";
            int rangecount = 0;
            foreach (var range in FreeTimeRange)
            {
                if (range == ',')
                {
                    WorkGraph[rangecount] = TimeRange;
                    rangecount++;
                    TimeRange = "";
                    continue;
                }
                TimeRange += range;
            }

            return WorkGraph;
        }
    }
}
