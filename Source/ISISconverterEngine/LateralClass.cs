using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class LateralClass
    {
        public string Keyword = "LATERAL";
        public string Label;
        public string Comment;
        public enum WeightTypes { reach, area, user  }
        public WeightTypes WeightType = WeightTypes.reach;
        public int NoReceiving = 0;
        public class ReceivingClass
        {
            public string Label = "";
            public double weight = 1;
            public bool Override = false;
        }
        public ReceivingClass[] ReceivingLocations;
        public LateralClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            Label = l.GetLabel(StArray[i], 1);
            i++;
            string stest = l.GetString(StArray[i], 1, ref OK);
            if (stest == "AREA") WeightType = WeightTypes.area;
            if (stest == "USER") WeightType = WeightTypes.user;
            i++;
            NoReceiving = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            ReceivingLocations = new ReceivingClass[NoReceiving];
            i++;
            int index = 0;
            for (int ii = i; ii < i + NoReceiving; ii++)
            {
                try
                {
                    ReceivingClass DataSet = new ReceivingClass();
                    DataSet.Label = l.GetString(StArray[ii], 1, ref OK);
                    DataSet.weight = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList);
                    stest = l.GetString(StArray[ii], 3, ref OK);
                    if (stest == "OVERRIDE") DataSet.Override = true;
                    ReceivingLocations[index] = DataSet;
                    index++;
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NoReceiving-1;
        }
    }
}
