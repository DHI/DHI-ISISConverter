using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MIKE11WaterLevelBoundaryClass : MIKE11BoundaryClass
    {
        public List<ValueTimePairClass> TimeSeries;
        public string dfs0Filename;
        public string ID;
        public MIKE11WaterLevelBoundaryClass(string riverName, double chainage, HtBoundaryClass HtBnd)
            : base(riverName, chainage)
        {
            BoundaryType = BoundaryTypes.Discharge;
            ID = HtBnd.ID.Labels[0];
            dfs0Filename = HtBnd.ID.Labels[0] + "_H.dfs0";
            TimeSeries = new List<ValueTimePairClass>();
            for (int i = 0; i < HtBnd.HtCurve.Count; i++)
            {
                ValueTimePairClass lpair = new ValueTimePairClass();
                lpair.Value = HtBnd.HtCurve[i].h;
                // values are WL - meters
                lpair.dateTime = new DateTime();
                lpair.dateTime = IsisDataClass.datetimeStart;
                double lsecs = HtBnd.TimeUnitInSeconds * (HtBnd.HtCurve[i].t);
                if (lsecs < 1e20)
                {
                    lpair.dateTime = lpair.dateTime.AddSeconds(lsecs);
                }
                else
                {
                    lpair.dateTime = lpair.dateTime.AddSeconds(0);
                }
                TimeSeries.Add(lpair);
            }
            DateTime endtime = new DateTime();
            endtime = IsisDataClass.datetimeStart;
            endtime = endtime.AddHours(IsisDataClass.duration);
            DateTime TSendtime = new DateTime();
            TSendtime = TimeSeries[TimeSeries.Count - 1].dateTime;
            if ((HtBnd.TimeExtension == "EXTEND") && (TSendtime < endtime))
            { // uses last time step plus duration to be on the safe side
                ValueTimePairClass lpair = new ValueTimePairClass();
                lpair.Value = TimeSeries[TimeSeries.Count - 1].Value;
                lpair.dateTime = new DateTime();
                lpair.dateTime = endtime;
                TimeSeries.Add(lpair);
            }
            if (HtBnd.TimeExtension == "REPEAT")
            {
                double FirtsTStep = HtBnd.HtCurve[1].t - HtBnd.HtCurve[0].t;
                double offsetsec = 0;
                bool hasErr = false;
                while (TSendtime < endtime && !hasErr)
                {
                    offsetsec = offsetsec + (HtBnd.HtCurve[HtBnd.HtCurve.Count - 1].t + FirtsTStep) * HtBnd.TimeUnitInSeconds;
                    for (int i = 0; i < HtBnd.HtCurve.Count; i++)
                    {
                        ValueTimePairClass lpair = new ValueTimePairClass();
                        lpair.Value = HtBnd.HtCurve[i].h;
                        lpair.dateTime = new DateTime();
                        lpair.dateTime = IsisDataClass.datetimeStart;
                        double lsecs = offsetsec + HtBnd.TimeUnitInSeconds * (HtBnd.HtCurve[i].t);
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

