using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class CulvertOutletClass : CulvertClass
    {
        public double OutletHeadLossCoeff_KO = 1.0;
        // Outlet head loss coefficient
        public CulvertOutletClass(string[] StArray, ref int i, ref List<int> errLineList)
            : base(StArray, ref i)
        {
            LineReaderClass l = new LineReaderClass();
            bool ok = true;
            OutletHeadLossCoeff_KO = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            string stest = l.GetString(StArray[i], 2, ref ok);
            if (stest == "CALCULATED") ReverseFlow = true;
            stest = l.GetString(StArray[i], 3, ref ok);
            if (stest != "TOTAL") HeadLossType = "ZERO";
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11EnergyLossClass M11EnergyLoss = new MIKE11EnergyLossClass(lstructure);
            M11EnergyLoss.Chainage = Chainage;
            M11EnergyLoss.RiverName = RiverName;
            M11EnergyLoss.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            MIKE11EnergyLossClass.LossCoeffClass LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = OutletHeadLossCoeff_KO;
            LossCoeff.LossNeg = OutletHeadLossCoeff_KO;
            M11EnergyLoss.Expansion = LossCoeff;
            return M11EnergyLoss;
        }
    }
}
