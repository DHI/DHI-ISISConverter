using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class PumpClass: StructureClass
    {
        //Full operating speed of pump (rpm)
        public double rpmful= 0;
        // Pumping head at optimum point of operation (m)
        public double  hopt;
        // Pump discharge at optimum point of operation (m3/s)
        public double qopt;
        // Pump efficiency at optimum point of operation (between 0.0 and 1.0)
        public double effopt;
        // Switch type keyword for pump unit - either 
        // `TIME' for pump controlled according to model time; 
        // `CONTROL' for pump under automatic control from control module unit(s); or 
        // `LOGICAL' for pump controlled by included logical RULES subunit
        public string swtype = "TIME";
        public int NoSwitches = 0;
        public double TimeUnitInSeconds = 1;
        public string TimeExtension = "";
        public enum pumpstates { OFF, ON, STOPPED };
        public enum Controlmodes { AUTO, MANUAL, CONTROLLER, LOGICAL } 
        public class PumpControleDataClass
        {
            public double t;
            public Controlmodes Controlemode = Controlmodes.AUTO;
            public pumpstates Pumpstate = pumpstates.ON;
        }
        public PumpControleDataClass[] ControlSwitchList;
        public int NoPumpCurveValues = 0;
        public class PumpCurveDataClass
        {
            public double head;
            public double flow;
            public double efficiency;
        }
        public PumpCurveDataClass[] PumpCurve;
        public PumpClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "OCPUMP";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            rpmful = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            hopt = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            qopt = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            effopt = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
            i++;
            swtype = l.GetString(StArray[i], 1, ref OK);
            i++;
            i++;
            NoSwitches = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            TimeUnitInSeconds = l.GetTimeUnitinSec(StArray[i], 2);
            TimeExtension = l.GetString(StArray[i], 3, ref OK);
            ControlSwitchList = new PumpControleDataClass[NoSwitches];
            for (int ii = 0; (ii < NoSwitches); ii++)
            {
                i++;
                PumpControleDataClass lswitchset = new PumpControleDataClass();
                lswitchset.t = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                string stest = l.GetString(StArray[i], 2, ref OK);
                switch (stest)
                {
                    case "AUTO":
                        {
                            lswitchset.Controlemode = Controlmodes.AUTO;
                            break;
                        }
                    case "MANUAL":
                        {
                            lswitchset.Controlemode = Controlmodes.MANUAL;
                            break;
                        }
                    default:
                        {
                            lswitchset.Controlemode = Controlmodes.AUTO;
                            break;
                        }
                }
                stest = l.GetString(StArray[i], 3, ref OK);
                switch (stest)
                {
                    case "OFF":
                        {
                            lswitchset.Pumpstate = pumpstates.OFF;
                            break;
                        }
                    case "ON":
                        {
                            lswitchset.Pumpstate = pumpstates.ON;
                            break;
                        }
                    case "STOPPED":
                        {
                            lswitchset.Pumpstate = pumpstates.STOPPED;
                            break;
                        }
                    default:
                        {
                            lswitchset.Pumpstate = pumpstates.ON;
                            break;
                        }
                }
                ControlSwitchList[ii] = lswitchset;
            }
            i++;
            i++;
            NoPumpCurveValues = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            PumpCurve = new PumpCurveDataClass[NoPumpCurveValues];
            for (int ii = 0; (ii < NoSwitches); ii++)
            {
                i++;
                PumpCurveDataClass lpumpdata = new PumpCurveDataClass();
                lpumpdata.head = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                lpumpdata.flow = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                lpumpdata.efficiency = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                if (!OK) lpumpdata.efficiency = effopt;
                PumpCurve[ii] = lpumpdata;
            }
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11PumpClass M11Pump = new MIKE11PumpClass(lstructure);
            M11Pump.Chainage = Chainage;
            M11Pump.RiverName = RiverName;
            M11Pump.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            M11Pump.SpecificationType = MIKE11PumpClass.SpecificationTypes.FixedDischarge;
            M11Pump.Discharge = qopt;
            return M11Pump;
        }
    }
}
