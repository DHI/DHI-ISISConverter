using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class RainEvapInfiltrationClass
    {
        public String Keyword = "REBDY";
        public string Comment = "";
        public string Label = "";
        public bool rain = false;
        public bool Evap = false;
        public bool Infil = false;
        

        public class ClimateDataPairClass
        {
            public double depth;
            public double t;
            public int hours = 0;
            public int minutes = 0;
            public int year = 1900;
            public int month = 1;
            public int day = 1;
        }
        
        public class ClimateDataClass
        {
        public int dataSets = 0;
        public bool DateTimeUsed = false;
        public double ConvergenceFactorIntensityTommPerHourForDateFormat = 1;
        public bool Intensity = true;
        public List<ClimateDataPairClass> ClimateDataPairCurve;
        public double TimeLag = 0;
        public double DatumAdjust = 0;
        public double TimeUnitInSeconds = 1;
        public string TimeExtension = "";
        public double multiplier = 1;
        }

        public ClimateDataClass RainSeries;
        public ClimateDataClass EvapSeries;
        public ClimateDataClass InfilSeries;


       private ClimateDataClass ReadClimateData(string[] StArray, ref int i, ref List<int> errLineList)
        {
            
           bool OK=true;    
           LineReaderClass l = new LineReaderClass();
          
           ClimateDataClass ClimateDataSeries = new ClimateDataClass();
           ClimateDataSeries.ClimateDataPairCurve = new List<ClimateDataPairClass>();
           ClimateDataSeries.dataSets = l.GetInt(StArray[i],1,i,ref OK, ref errLineList);
            ClimateDataSeries.TimeLag = l.GetDouble(StArray[i],2,i,ref OK, ref errLineList);
            if (!OK) ClimateDataSeries.TimeLag = 0; 
           ClimateDataSeries.DatumAdjust = l.GetDouble(StArray[i],3,i,ref OK, ref errLineList);
            if (!OK) ClimateDataSeries.DatumAdjust = 0;
           string stest = l.GetString(StArray[i], 4, ref OK);
           if (stest == "DATE") 
           { 
               ClimateDataSeries.DateTimeUsed = true;
           }
           else
           {
               ClimateDataSeries.TimeUnitInSeconds = l.GetTimeUnitinSec(StArray[i],4);
           }
           ClimateDataSeries.TimeExtension = l.GetTimeExtensionMethod(StArray[i],5, ref OK);
           // the interpolation method switch is ignored since this is not used in the data file
           ClimateDataSeries.multiplier = l.GetDouble(StArray[i], 7, i, ref OK, ref errLineList);
            if (!OK) ClimateDataSeries.multiplier = 1;
           i++;
           ClimateDataSeries.ConvergenceFactorIntensityTommPerHourForDateFormat = l.GetTimeUnitinSec(StArray[i], 1) / 60 / 60;
           stest = l.GetString(StArray[i], 2, ref OK);
           if (stest == "DEPTH")
           {
               ClimateDataSeries.Intensity = false;
           }
           i++;

                 
                 for(int ii=i;ii<i+ClimateDataSeries.dataSets;ii++)
                     {
                        try
                        {
                            ClimateDataPairClass Data = new ClimateDataPairClass();
                            Data.depth = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList);
                            if (ClimateDataSeries.DateTimeUsed)
                            {
                                Data.hours = l.GetHour(StArray[ii], 2, ref OK);
                                Data.minutes = l.GetMinutes(StArray[ii], 2, ref OK);
                                Data.day = l.GetDay(StArray[ii], 3, ref OK);
                                Data.month = l.GetMonth(StArray[ii], 3, ref OK);
                                Data.year = l.GetYear(StArray[ii], 3, ref OK);
                            }
                            else
                            {
                                Data.t = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList);
                            }

                            ClimateDataSeries.ClimateDataPairCurve.Add(Data);
                        }
                        catch(Exception e)
                        {
                            i = ii;
                            throw e;
                        }
                     }

                 i = i + ClimateDataSeries.dataSets;

                 return ClimateDataSeries;
        
       }


       public RainEvapInfiltrationClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
           bool OK = true;
           LineReaderClass l = new LineReaderClass();
           Comment = l.GetComment(Keyword, StArray[i]);
           i++;
           Label = l.GetLabel(StArray[i], 1);
           i++;
           for (int ii = 1; ii < 4; ii++)
           {
               string stest = l.GetString(StArray[i], ii, ref OK);
               switch (stest)
               {
                   case "RAIN":
                       {
                           rain = true;
                           break;
                       }
                   case "EVAP":
                       {
                           Evap = true;
                           break;
                       }
                   case "INFI":
                       {
                           Infil = true;
                           break;
                       }

                   default:
                       break;
               }
           }
           i++;
           if (rain)
           {
               RainSeries =ReadClimateData(StArray, ref i, ref errLineList);
           }
           if (Evap)
           {
               EvapSeries = ReadClimateData(StArray, ref i, ref errLineList);
           }
           if (Infil)
           {
               InfilSeries = ReadClimateData(StArray, ref i, ref errLineList);
           }
       } 
        
    }
}
