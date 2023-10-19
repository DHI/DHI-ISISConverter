using System;
using System.Collections.Generic;

namespace ISISConverterEngine
{
    public class HtBoundaryClass
    {
        public String Keyword = "HTBDY";
        public string Comment = "";
        public LabelCollectionClass ID;
        private int NdataSets = 0;
        public double TimeUnitInSeconds = 1;
        public double datumshift = 0;
        public string TimeExtension = "";
        public class HtDataClass
        {
            public double h = 0;
            public double t = 0;

        }
        public List<HtDataClass> HtCurve;
        public HtBoundaryClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            NdataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            datumshift = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            if (!OK) datumshift = 0;
            TimeUnitInSeconds = l.GetTimeUnitinSec(StArray[i], 3);
            TimeExtension = l.GetTimeExtensionMethod(StArray[i], 4, ref OK);
            i++;
            HtCurve = new List<HtDataClass>();
            for (int ii = i; ii < i + NdataSets; ii++)
            {
                try
                {
                    HtDataClass HtData = new HtDataClass();
                    HtData.h = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList);
                    HtData.t = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList);

                    HtCurve.Add(HtData);
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NdataSets - 1;

        }



    }
}
