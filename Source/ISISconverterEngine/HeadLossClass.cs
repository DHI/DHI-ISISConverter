using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class HeadLossClass: StructureClass
    {
        public bool Upstream = true;
        public bool Reverse = true;
        public double HeadLossCoeff_K = 1;



        public HeadLossClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "LOSS";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            string stest = l.GetString(StArray[i], 1, ref OK);
            if (stest == "DOWNSTREAM") Upstream = false;
            i++;
            HeadLossCoeff_K = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            stest = l.GetString(StArray[i], 2, ref OK);
            if (stest == "ZERO") Reverse = false;
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11EnergyLossClass M11EnergyLoss = new MIKE11EnergyLossClass(lstructure);
            M11EnergyLoss.Chainage = Chainage;
            M11EnergyLoss.RiverName = RiverName;
            M11EnergyLoss.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            MIKE11EnergyLossClass.LossCoeffClass LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = HeadLossCoeff_K;
            LossCoeff.LossNeg = HeadLossCoeff_K;
            M11EnergyLoss.Userdefined = LossCoeff;
            return M11EnergyLoss;
        }


    }
}
