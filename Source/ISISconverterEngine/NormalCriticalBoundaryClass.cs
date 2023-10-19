using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class NormalCriticalBoundaryClass
    {

        public class SurveydataClass
        {
            public double x = 0, z = 0, n = 0; 
            public bool Panel =false;
        }

        public SurveydataClass[] Surveydata;
        
        public String Keyword = "NCDBDY";
        public string Comment = "";
        public string Label = "";
        public string Label2 = "";
        public string Label3 = "";
        public string Label4 = "";
        public bool Normal = true;
        public bool AutomaticSlopeCalculation = true;
        public bool BedSlope = true;
        public double slope = 0;
        public int nDataSurvey = 0;

        public NormalCriticalBoundaryClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            Label = l.GetLabel(StArray[i], 1);
            Label2 = l.GetLabel(StArray[i], 2);
            Label3 = l.GetLabel(StArray[i], 3);
            Label4 = l.GetLabel(StArray[i], 4);
            i++;
            bool ok = true;
            string stest = l.GetString(StArray[i],1,ref ok);
            if (!ok) stest = "NORMAL";
            if (stest != "NORMAL")
            {
                Normal = false;
            }
            else
            {
                i++;
                stest = l.GetString(StArray[i], 1, ref ok);
                if (!ok) stest = "AUTO";
                if (stest != "AUTO")
                {
                    slope = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
                    if (!ok) slope =0;
                }
                else 
                {
                    stest = l.GetString(StArray[i], 2, ref ok);
                    if (!ok) stest = "BED";
                    if (stest != "BED")
                    {
                        BedSlope = false;
                    }

                }
            }
            i++;
            nDataSurvey = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            if (!ok) nDataSurvey = 0;
            Surveydata = new SurveydataClass[nDataSurvey];
            i++;
            int index = 0;
            for (int ii=i;ii<i+nDataSurvey;ii++)
            {
                try
                {
                    Surveydata[index].x = l.GetDouble(StArray[ii], 1, ii, ref ok, ref errLineList);
                    Surveydata[index].z = l.GetDouble(StArray[ii], 2, ii, ref ok, ref errLineList);
                    Surveydata[index].n = l.GetDouble(StArray[ii], 3, ii, ref ok, ref errLineList);
                    stest = l.GetString(StArray[ii], 4, ref ok);
                    if (ok && stest == "*") Surveydata[index].Panel = true;
                    index++;
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + nDataSurvey;
            int novdat = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            i = i + novdat - 1;
            // ignore section on override possibility

            
            
 
          }
        
    }
}
