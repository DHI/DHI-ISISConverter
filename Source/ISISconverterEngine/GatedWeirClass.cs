using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class WaterLevelGateOpeningSetClass
    {
        public double WaterLevel, GateOpening;
    }

    

    public class GatedWeirClass : StructureClass
    {
        public double Ctc, Cgt, Crev, m;
        public bool ForwardOrientation = false;
        public double b, zc, hg;
        public int BIAS;
        public enum OperationModeTypes { time, water1, water2, water3, control, logical }
        public OperationModeTypes OperationMode = OperationModeTypes.time;
        public int NOperations=0;
        public WaterLevelGateOpeningSetClass[]  WaterLevelGateopeningCurve;

        public GatedWeirClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "GATED WEIR";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            bool ok = true;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            Ctc = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            Cgt = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            Crev = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            m = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
            ok = true;
            string test = l.GetString(StArray[i], 5, ref ok);
            if (test == "FORWARD") ForwardOrientation = true;
            i++;
            b = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            zc = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            hg = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            BIAS = l.GetInt(StArray[i], 4, i, ref OK, ref errLineList);
            i++;
            i++;
            string test2 = l.GetString(StArray[i], 1, ref ok);
            switch (test2)
            {

                case "time":
                    {
                        OperationMode = OperationModeTypes.time;
                        break;
                    }
                case "water1":
                    {
                        OperationMode = OperationModeTypes.water1;
                        break;
                    }
                case "water2":
                    {
                        OperationMode = OperationModeTypes.water2;
                        break;
                    }
                case "water3":
                    {
                        OperationMode = OperationModeTypes.water3;
                        break;
                    }
                case "control":
                    {
                        OperationMode = OperationModeTypes.control;
                        break;
                    }
                case "logical":
                    {
                        OperationMode = OperationModeTypes.logical;
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
            i++;
            i++;
            
                NOperations = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                WaterLevelGateopeningCurve = new WaterLevelGateOpeningSetClass[NOperations];
                for (int iii = 0; iii < NOperations; iii++)
                {
                    i++;
                    if ((OperationMode != OperationModeTypes.control) && (OperationMode != OperationModeTypes.time))
                    {
                        WaterLevelGateOpeningSetClass lwaterLevelGateOpening = new WaterLevelGateOpeningSetClass();
                        lwaterLevelGateOpening.WaterLevel = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                        lwaterLevelGateOpening.GateOpening = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                        WaterLevelGateopeningCurve[iii] = lwaterLevelGateOpening;
                    }
                }


            }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11ControlStructureClass M11Controlstructure = new MIKE11ControlStructureClass(lstructure);
            M11Controlstructure.Chainage = Chainage;
            M11Controlstructure.RiverName = RiverName;
            M11Controlstructure.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            M11Controlstructure.ControlStrucType = MIKE11ControlStructureClass.ControlStrucTypes.Overflow;
            M11Controlstructure.GateWidth = b;
            M11Controlstructure.SillLevel = zc;
            M11Controlstructure.NoGates = 1;
            return M11Controlstructure;
        }
        }
    }

