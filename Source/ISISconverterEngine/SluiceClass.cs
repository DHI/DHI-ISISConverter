using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class WaterLevelOperationSetClass
    {
        public double WaterLevel, GateLevel;
    }

    public class SingleGateClass
    {
        public int Noperations = 0;
        public WaterLevelOperationSetClass[] WaterLevelOperationData; 
    }

    public class SluiceClass : StructureClass
    {
        public enum SluiceTypes {Radial, Vertical}
        public SluiceTypes SluiceType = SluiceTypes.Radial;
        public double Cvw, Cvg, b, zc, hg, L;
        public bool UseDegrees = false;
        public double p1, p2;
        public double BIAS;
        public double Cvs, hp, R;
        public int Ngates = 1;
        public enum OperationModeTypes { time, water1, water2, water3, control, logical }
        public OperationModeTypes OperationMode = OperationModeTypes.time;
        public SingleGateClass[] GateData; 
   
    public SluiceClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
        Keyword = "SLUICE";    
        bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            bool ok = true;
            Keyword2 = l.GetString(StArray[i], 1, ref ok);
            if (Keyword2 == "VERTICAL")
            {
                SluiceType = SluiceTypes.Vertical;
            }
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            Cvw = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            Cvg = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            b = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            zc = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
            hg = l.GetDouble(StArray[i], 5, i, ref OK, ref errLineList);
            L = l.GetDouble(StArray[i], 6, i, ref OK, ref errLineList);
            if (SluiceType == SluiceTypes.Radial)
            {
                ok = true;
                string test = l.GetString(StArray[i], 7, ref ok);
                if (test == "DEGREES") UseDegrees = true; 
            }
            i++;
            p1 = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            p2 = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            BIAS = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            Cvs = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
            if (SluiceType == SluiceTypes.Radial)
            {
                ok = true;
                hp = l.GetDouble(StArray[i], 5, i, ref OK, ref errLineList);
                R = l.GetDouble(StArray[i], 6, i, ref OK, ref errLineList);
            }
            i++;
            Ngates = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            i++;
            string test2 = l.GetString(StArray[i], 1, ref ok);
            switch (test2)
            {

                case "TIME":
                    {
                        OperationMode = OperationModeTypes.time;
                        break;
                    }
                case "WATER1":
                    {
                        OperationMode = OperationModeTypes.water1;
                        break;
                    }
                case "WATER2":
                    {
                        OperationMode = OperationModeTypes.water2;
                        break;
                    }
                case "WATER3":
                    {
                        OperationMode = OperationModeTypes.water3;
                        break;
                    }
                case "CONTROL":
                    {
                        OperationMode = OperationModeTypes.control;
                        break;
                    }
                case "LOGICAL":
                    {
                        OperationMode = OperationModeTypes.logical;
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
            
            for (int ii = 0; ii < Ngates; ii++)
            {
                i++;
                SingleGateClass lGate = new SingleGateClass();
                i++;
                lGate.Noperations = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                lGate.WaterLevelOperationData = new WaterLevelOperationSetClass[lGate.Noperations];
                for (int iii = 0; iii < lGate.Noperations; iii++)
                {
                    i++;
                    if (((OperationMode != OperationModeTypes.control) && (OperationMode != OperationModeTypes.time)) && (OperationMode != OperationModeTypes.logical))
                    {
                        WaterLevelOperationSetClass lwaterLevelGateLevel = new WaterLevelOperationSetClass();
                        lwaterLevelGateLevel.WaterLevel = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                        lwaterLevelGateLevel.GateLevel = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                        lGate.WaterLevelOperationData[iii] = lwaterLevelGateLevel;
                    }
                }

 
            }

        }
    public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
    {
        MIKE11ControlStructureClass M11Controlstruc = new MIKE11ControlStructureClass(lstructure);
        M11Controlstruc.Chainage = Chainage;
        M11Controlstruc.RiverName = RiverName;
        M11Controlstruc.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
        if (SluiceType == SluiceTypes.Vertical)
        {
            M11Controlstruc.ControlStrucType = MIKE11ControlStructureClass.ControlStrucTypes.Underflow;
            M11Controlstruc.UnderflowCc = this.Cvs;
        }
        else
        {
            M11Controlstruc.ControlStrucType = MIKE11ControlStructureClass.ControlStrucTypes.RadialGate;
            M11Controlstruc.Height = hg;
            M11Controlstruc.Radius = R;
            M11Controlstruc.Trunnion = hp; 
        }
        M11Controlstruc.GateWidth = b;
        M11Controlstruc.SillLevel = zc;
        return M11Controlstruc;
    }
    }


}
