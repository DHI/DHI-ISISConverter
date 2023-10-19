using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class FloodPlainClass

    {
        public String Keyword = "UNDEFINED";
        public String Keyword2;
        public string Comment = "";
        public LabelCollectionClass ID;
        public double WeirCoeff_Cd = 1;
        // Weir coefficient (includes discharge, velocity and calibration coefficients)
        public double ModularLimit_m = 0.9;
        // Modular limit (eg 0.9)
        public double Distance_d1;
        // Distance from centre of upstream cell to section (m)
        public double Distance_d2;
        // Distance from section to centre of downstream cell (m)
        public bool UseFriction = false;
        // Optional keyword 'FRICTION' to force friction flow for all segments
        public double MiminmumArea_ds_constraint = 0.1; 
        // Minimum value of downstream area (relative to upstream area) when Manning’s equation applies. Typical value 0.1.
        public int NoDatasets = 0;
        class FloodplainXsecDataClass
        {
            public double x, y, n;
        }
        FloodplainXsecDataClass[] FloodPlainXsection;



        public FloodPlainClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "FLOODPLAIN";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            Keyword2 = l.GetString(StArray[i], 1, ref OK);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            WeirCoeff_Cd = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            ModularLimit_m = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            Distance_d1 = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            Distance_d2 = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
            string stest = l.GetString(StArray[i], 5, ref OK);
            if (stest == "FRICTION") UseFriction = true;
            MiminmumArea_ds_constraint = l.GetDouble(StArray[i], 6, i, ref OK, ref errLineList);
            i++;
            NoDatasets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            FloodPlainXsection = new FloodplainXsecDataClass[NoDatasets];
            i++;
            int index = 0;
            for (int ii = i; ii < i + NoDatasets; ii++)
            {
                try
                {
                    FloodplainXsecDataClass RawDataSet = new FloodplainXsecDataClass();
                    bool ok = true;
                    RawDataSet.x = l.GetDouble(StArray[ii], 1, ii, ref ok, ref errLineList);
                    RawDataSet.y = l.GetDouble(StArray[ii], 2, ii, ref ok, ref errLineList);
                    RawDataSet.n = l.GetDouble(StArray[ii], 3, ii, ref ok, ref errLineList);
                    FloodPlainXsection[index] = RawDataSet;
                    index++;
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NoDatasets-1; 
          
        }
        
        
    }
}
