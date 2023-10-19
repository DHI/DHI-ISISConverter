using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    class USBPRBridgeClass : BridgeClass
    {
        public double PierWidth = 0;
        // total width of piers normal to flow direction (m)
        public int iabutment = 3;
        // abutment type identifier. 
        // Use a value of 3 except if the span of the bridge between the abutments is less than 60m 
        // and there is either a 90 degree wing or vertical wall abutment (iabut=1) or a 30 degree wing wall abutment (iabut=2)

        public int NoPier = 0;
        public string shape = "FLAT";
        // if no piers present then enter shape of bridge soffit as either FLAT (if rectangular) or ARCH (if not rectangular). 
        // If piers are present then enter their cross sectional shape as either RECTANGLE, CYLINDER, SQUARE or I (for I-beam piles). 
        // Enter COEF if a pier coefficient is to be used instead of pier shape description. 
        // Single I-beams and square piers are not covered by the theory, and are treated as rectangular. 
        // Twin I-beams and rectangular piers are also not covered, and are treated as twin square piers


        public string diaph = "STRMLINE";
        // second description of bridge pier cross sectional shape (for possible use when there are 1 or 2 piers). Options are:
        // STRMLINE - streamline pier faces
        // SEMICIRCLE - semi-circular pier faces
        // TRIANGLE - triangular pier faces
        // DIAPHRAGM - diaphragm wall between piers. A diaphragm with only one pier is not possible and is ignored

        public double PierCoef = 0;
        // calibration number of piers. Used if shape is set to COEF. 
        // This is a real number between 0.0 and 8.0 representing the streamlining of the piers. 
        // Zero represents no pier resistance, 8 gives the maximum resistance, representing several I-beam piles. 
        public string AlignmentType = "ALIGNED";
        // indicator for abutments being aligned with normal direction of flow, ALIGNED or SKEW

        public int NoCulverts = 0;
        // Flood relief culverts are assumed to be rectangular
        
        public class USBPRCulvertClass
        {
            public double InvertLevel = 0;
            // invert level of culvert (m AD)
            public double SoffitLevel = 0;
            // soffit level of culvert (m AD)
            public double CrossSectionArea = 0;
            // cross section area of culvert (m2)
            public double PartFullDischargeCoeff = 1;
            // part full discharge coefficient
            public double FullDischargeCoeff = 1;
            // full flow discharge coefficient
            public double DrowningCoeff = 0;
            // drowning coefficient for part full flow (must be <1)
        }

        public USBPRCulvertClass[] USBPRCulverts;


        public USBPRBridgeClass(string[] StArray, ref int i, ref List<int> errLineList)
            : base(StArray, ref i, ref errLineList)
        {
            LineReaderClass l = new LineReaderClass();
            bool ok = true;
            PierWidth = l.GetDouble(StArray[i], 5, i, ref ok, ref errLineList);
            i++;
            iabutment = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            i++;
            NoPier = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            shape = l.GetString(StArray[i], 2, ref ok);
            diaph = l.GetString(StArray[i], 3, ref ok);
            if (shape == "COEF") PierCoef = l.GetDouble(StArray[i], 4, i, ref ok, ref errLineList);
            i++;
            AlignmentType = l.GetString(StArray[i], 1, ref ok);
            i++;
            ReadRawData(StArray, ref i, ref errLineList);
            ReadArchData(StArray, ref i, ref errLineList);
            NoCulverts = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            USBPRCulverts = new USBPRCulvertClass[NoCulverts];
            int index = 0;
            for (int ii = i+1; ii < i + NoCulverts; ii++)
            {
                try
                {
                    USBPRCulvertClass CulvertDataSet = new USBPRCulvertClass();
                    CulvertDataSet.InvertLevel = l.GetDouble(StArray[ii], 1, ii, ref ok, ref errLineList);
                    CulvertDataSet.SoffitLevel = l.GetDouble(StArray[ii], 2, ii, ref ok, ref errLineList);
                    CulvertDataSet.CrossSectionArea = l.GetDouble(StArray[ii], 3, ii, ref ok, ref errLineList);
                    CulvertDataSet.PartFullDischargeCoeff = l.GetDouble(StArray[ii], 4, ii, ref ok, ref errLineList);
                    CulvertDataSet.FullDischargeCoeff = l.GetDouble(StArray[ii], 5, ii, ref ok, ref errLineList);
                    CulvertDataSet.DrowningCoeff = l.GetDouble(StArray[ii], 6, ii, ref ok, ref errLineList);
                    USBPRCulverts[index] = CulvertDataSet;
                    index++;
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NoCulverts; 

        }
    }
}
