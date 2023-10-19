using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class BridgeClass: StructureClass
    {

        public class BridgeRawDataSetClass
        {
            public double dx=0, dy=0, dn=0;
            public bool bLeft = false;
            public bool bRight = false;
        }

        public class BridgeArchRawDataClass
        {
            public double LeftX = 0;
            public double RightX = 0;
            public double SpringingLevel = 0;
            public double SoffitLevel = 0;
        }
        public double dCalibrationFactor  = 1;
        public double dSkewnessAngle = 0;
        public bool bOrifice = false;
        public double dTransistionLower = 0;
        public double dTransistionUpper = 0;
        public double dDischargeCoefficientOrifice = 1;
        public double dBridgeWidthFlowDirection = 0;
        public double dBridgeDistance = 0;
        
        public int NoPoints = 0;
        public BridgeRawDataSetClass[] Rawdata; 
        public void ReadRawData(string[] StArray, ref int i, ref List<int> errLineList)
        {
            LineReaderClass l = new LineReaderClass();
            bool ok = true;
            NoPoints = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            Rawdata = new BridgeRawDataSetClass[NoPoints];
            i++;
            int index = 0;
            for (int ii = i; ii < i + NoPoints; ii++)
            {
                try
                {
                    BridgeRawDataSetClass RawDataSet = new BridgeRawDataSetClass();
                    RawDataSet.dx = l.GetDouble(StArray[ii], 1, ii, ref ok, ref errLineList);
                    RawDataSet.dy = l.GetDouble(StArray[ii], 2, ii, ref ok, ref errLineList);
                    RawDataSet.dn = l.GetDouble(StArray[ii], 3, ii, ref ok, ref errLineList);
                    string stest = l.GetString(StArray[ii], 4, ref ok);
                    if (stest == "L") RawDataSet.bLeft = true;
                    if (stest == "R") RawDataSet.bRight = true;
                    Rawdata[index] = RawDataSet;
                    index++;
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NoPoints; 
        }

        public int NoArches = 0;
        public BridgeArchRawDataClass[] ArchData;
        public void ReadArchData(string[] StArray, ref int i, ref List<int> errLineList)
        {
            LineReaderClass l = new LineReaderClass();
            bool ok = true;
            NoArches = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            ArchData = new BridgeArchRawDataClass[NoArches];
            i++;
            int index = 0;
            for (int ii = i; ii < i + NoArches; ii++)
            {
                try
                {
                    BridgeArchRawDataClass ArchDataSet = new BridgeArchRawDataClass();
                    ArchDataSet.LeftX = l.GetDouble(StArray[ii], 1, ii, ref ok, ref errLineList);
                    ArchDataSet.RightX = l.GetDouble(StArray[ii], 2, ii, ref ok, ref errLineList);
                    ArchDataSet.SpringingLevel = l.GetDouble(StArray[ii], 3, ii, ref ok, ref errLineList);
                    ArchDataSet.SoffitLevel = l.GetDouble(StArray[ii], 4, ii, ref ok, ref errLineList);
                    ArchData[index] = ArchDataSet;
                    index++;
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NoArches;
        }

        public BridgeClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "BRIDGE";
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            bool ok = true;
            Keyword2 = l.GetString(StArray[i], 1, ref ok);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            i++;
            dCalibrationFactor = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            dSkewnessAngle = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
            dBridgeWidthFlowDirection = l.GetDouble(StArray[i],3,i, ref ok, ref errLineList);
            dBridgeDistance = l.GetDouble(StArray[i],4,i, ref ok, ref errLineList);
            string stest = l.GetString(StArray[i], 6, ref ok);
            if (stest == "ORIFICE") bOrifice = true;
            if (bOrifice)
            {
                dTransistionLower = l.GetDouble(StArray[i], 7, i, ref ok, ref errLineList);
                dTransistionUpper = l.GetDouble(StArray[i], 8, i, ref ok, ref errLineList);
                dDischargeCoefficientOrifice = l.GetDouble(StArray[i], 9, i, ref ok, ref errLineList);
            }



 
            }
        
    }
}
