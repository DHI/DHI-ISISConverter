using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class TidalBoundaryClass
    {
        public String Keyword = "TIDBDY";
        public string Comment = "";
        public string Label = "";
        private int NdataSets = 0;
        public double datumshift = 0;
        public double StartTimeFromSimStart = 0;
        public double SurgeDuration = 0;
        public double SurgeAmplitude = 0;
        public double SurgePower = 1;
        public double MeanSeaLevel = 0;
        public double hour  = 0;
        public int integerdate = 101; // mmdd jan/1 101  
        public int startyear = 1900;
        public string sourcetidbdy_dates = "";

        
        public class HarmonicsDataClass
        {
            public string constitutename = "";
            public double amplitude= 0;
            public double phasedegrees= 0;
        }
        public List<HarmonicsDataClass> HarmConstituents;
        public TidalBoundaryClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            bool OK=true;    
            LineReaderClass l = new LineReaderClass();
                Comment = l.GetComment(Keyword, StArray[i]);
                i++;
                Label = l.GetLabel(StArray[i], 1);
                i++; 
                datumshift = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                if (!OK) datumshift=0;
                i++;
                StartTimeFromSimStart = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                SurgeDuration = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                SurgeAmplitude = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                SurgePower = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
                i++;
                MeanSeaLevel = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                i++;
                hour = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                integerdate = l.GetInt(StArray[i], 2, i, ref OK, ref errLineList);
                startyear = l.GetInt(StArray[i], 3, i, ref OK, ref errLineList);
                sourcetidbdy_dates = l.GetString(StArray[i], 4, ref OK);
                if (!(sourcetidbdy_dates == "EVENT")) { sourcetidbdy_dates = ""; };
                 i++;
                 NdataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                i++;
                 for(int ii=i;ii<i+NdataSets;ii++)
                     {
                        try
                        {
                             HarmonicsDataClass HarmData = new HarmonicsDataClass();
                             StArray[ii] = "      "+StArray[ii];
                             HarmData.constitutename = l.GetString(StArray[ii],1,ref OK);    
                             HarmData.amplitude = l.GetDouble(StArray[ii],2, ii, ref OK, ref errLineList);
                            HarmData.phasedegrees = l.GetDouble(StArray[ii],3, ii, ref OK, ref errLineList);
                            HarmConstituents.Add(HarmData);
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
