using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class ReservoirClass
    {
         
        String Keyword = "RESERVOIR";
        public string Comment {get; private set;}
        public List<string> LabelCollection { get; private set; }
        public List<string> LateralLabelCollection { get; private set; }
        bool UseRevision = false;
        public int NoDataSets { get; private set; }
        public double easting { get; private set; }
        public double northing { get; private set; }
        public double RunoffFactor { get; private set; }
        public class StageAreaClass
        {
            public double h { get; set; }
            public double A { get; set; }
        }
        public StageAreaClass[] StageAreaCurve { get; private set; }
        public ReservoirClass(string[] StArray, ref int i, ref List<int> errLineList)
            {
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            UseRevision = Comment.Contains("#revision#");
            i++;
            bool OK = false;
            LabelCollection = new List<string>();
            string stest = "NOTAssigned";
            int ii =1;
            while (!(stest == ""))
            {
                stest = l.GetLabel(StArray[i], ii);
                if (!(stest == ""))
                {
                    LabelCollection.Add(stest);
                }
                ii++;
            }
            if (UseRevision)
            {
                LateralLabelCollection = new List<string>();
                i++;
                stest = "NOTAssigned";
                ii = 1;
                while (!(stest == ""))
                {
                    stest = l.GetLabel(StArray[i], ii);
                    if (!(stest == ""))
                    {
                        LateralLabelCollection.Add(stest);
                    }
                    ii++;
                }

            }
            i++;
            NoDataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            StageAreaCurve = new StageAreaClass[NoDataSets];
            for (ii = 0; (ii < NoDataSets); ii++)
            {
                i++;
                StageAreaClass lhA = new StageAreaClass();
                lhA.h = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                lhA.A = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                StageAreaCurve[ii] = lhA;
            }
            if (UseRevision)
            {
                i++;
                easting = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                northing = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                RunoffFactor = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                if (!OK) RunoffFactor = 0;

            }
            }
    }
}
