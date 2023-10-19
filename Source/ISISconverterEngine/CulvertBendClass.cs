using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class CulvertBendClass : CulvertClass
    {
        public double BendHeadLossCoeff_KB = 0;
        public bool Upstream = true;
        public CulvertBendClass(string[] StArray, ref int i, ref List<int> errLineList)
            : base(StArray, ref i)
        {
            LineReaderClass l = new LineReaderClass();
            bool ok = true;
            string stest = l.GetString(StArray[i], 1, ref ok);
            if (stest == "DOWNSTREAM") Upstream = false;
            i++;
            BendHeadLossCoeff_KB = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            stest = l.GetString(StArray[i], 2, ref ok);
            if (stest == "CALCULATED") ReverseFlow = true;
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11EnergyLossClass M11EnergyLoss = new MIKE11EnergyLossClass(lstructure);
            M11EnergyLoss.Chainage = Chainage;
            M11EnergyLoss.RiverName = RiverName;
            M11EnergyLoss.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            MIKE11EnergyLossClass.LossCoeffClass LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = BendHeadLossCoeff_KB;
            LossCoeff.LossNeg = BendHeadLossCoeff_KB;
            M11EnergyLoss.Userdefined = LossCoeff;
            return M11EnergyLoss;
        }

    }
}
