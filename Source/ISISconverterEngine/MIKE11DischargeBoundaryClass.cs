using System;
using System.Collections.Generic;

namespace ISISConverterEngine
{
    class MIKE11DischargeBoundaryClass : MIKE11BoundaryClass
    {
        public List<ValueTimePairClass> TimeSeries;
        public string dfs0Filename;
        public string ID;
        public MIKE11DischargeBoundaryClass(string riverName, double chainage, QtBoundaryClass QtBnd)
            : base(riverName, chainage)
        {
            BoundaryType = BoundaryTypes.Discharge;
            ID = QtBnd.ID.Labels[0];
            dfs0Filename = QtBnd.ID.Labels[0] + "_Q.dfs0";
            TimeSeries = new List<ValueTimePairClass>();
            for (int i = 0; i < QtBnd.QtCurve.Count; i++)
            {
                ValueTimePairClass lpair = new ValueTimePairClass();
                lpair.Value = QtBnd.QtCurve[i].Q * QtBnd.QMultiplier;
                // values are Q - m3/s
                lpair.dateTime = new DateTime();
                lpair.dateTime = IsisDataClass.datetimeStart;
                double lsecs = QtBnd.TimeUnitInSeconds * (QtBnd.TimeLag + QtBnd.QtCurve[i].t);
                lpair.dateTime = lpair.dateTime.AddSeconds(lsecs);
                TimeSeries.Add(lpair);
            }
            DateTime endtime = new DateTime();
            endtime = IsisDataClass.datetimeStart;
            endtime = endtime.AddHours(IsisDataClass.duration);
            DateTime TSendtime = new DateTime();
            TSendtime = TimeSeries[TimeSeries.Count - 1].dateTime;
            if ((QtBnd.TimeExtension == "EXTEND") && (TSendtime < endtime))
            { // uses last time step plus duration to be on the safe side
                ValueTimePairClass lpair = new ValueTimePairClass();
                lpair.Value = TimeSeries[TimeSeries.Count - 1].Value;
                lpair.dateTime = new DateTime();
                lpair.dateTime = endtime;
                TimeSeries.Add(lpair);
            }
            if (QtBnd.TimeExtension == "REPEAT")
            {
                double FirtsTStep = QtBnd.QtCurve.Count > 1 ? QtBnd.QtCurve[1].t - QtBnd.QtCurve[0].t : 0;
                double offsetsec = QtBnd.TimeUnitInSeconds * QtBnd.TimeLag;
                bool hasErr = false;
                while (TSendtime < endtime && !hasErr)
                {
                    offsetsec = offsetsec + (QtBnd.QtCurve[QtBnd.QtCurve.Count - 1].t + FirtsTStep) * QtBnd.TimeUnitInSeconds;
                    for (int i = 0; i < QtBnd.QtCurve.Count; i++)
                    {
                        ValueTimePairClass lpair = new ValueTimePairClass();
                        lpair.Value = QtBnd.QtCurve[i].Q * QtBnd.QMultiplier;
                        lpair.dateTime = new DateTime();
                        lpair.dateTime = IsisDataClass.datetimeStart;
                        double lsecs = offsetsec + QtBnd.TimeUnitInSeconds * (QtBnd.QtCurve[i].t);
                        if (lsecs <= 0)
                        {
                            hasErr = true;
                            break;
                        }
                        lpair.dateTime = lpair.dateTime.AddSeconds(lsecs);
                        TimeSeries.Add(lpair);
                    }
                    TSendtime = TimeSeries[TimeSeries.Count - 1].dateTime;

                }
            }
        }
    }

}
