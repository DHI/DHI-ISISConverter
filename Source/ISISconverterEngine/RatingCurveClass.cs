using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class RatingCurveClass
    {
        public string Keyword = "QRATING";
        public string comment = "";
        public string label = "";
        public int NoDatasets = 0;
        public class QhdatasetClass
        {
            public double Q = 0;
            public double h = 0;
        }
        public QhdatasetClass[] Ratingcurve;
        public RatingCurveClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            comment = l.GetComment(Keyword, StArray[i]);
            i++;
            label = l.GetLabel(StArray[i], 1);
            i++;
            NoDatasets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            Ratingcurve = new QhdatasetClass[NoDatasets];
            for (int ii = 0; (ii < NoDatasets); ii++)
            {
                QhdatasetClass lqhset = new QhdatasetClass();
                lqhset.Q = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                lqhset.h = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                Ratingcurve[ii] = lqhset;
            }
        }


    }
}
