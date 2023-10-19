using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class ManholeClass: StructureClass
    {
        public double Elevation_z = 0;
        public double Diameter_dia = 1;
        public double calibration_coeff = 1.7;
        // Lumped calibration coefficient (includes discharge coefficient, constants [2/3,g],etc)
        public double ModularLimit_r = 0.9;
        // 0<r<1
        public double LossCoeff_K = 0;
        public ManholeClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "MANHOLE";
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            bool ok = true;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            Elevation_z = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            Diameter_dia = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
            calibration_coeff  = l.GetDouble(StArray[i], 3, i, ref ok, ref errLineList);
            ModularLimit_r = l.GetDouble(StArray[i], 4, i, ref ok, ref errLineList);
            LossCoeff_K = l.GetDouble(StArray[i], 5, i, ref ok, ref errLineList);
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11EnergyLossClass M11EnergyLoss = new MIKE11EnergyLossClass(lstructure);
            M11EnergyLoss.Chainage = Chainage;
            M11EnergyLoss.RiverName = RiverName;
            M11EnergyLoss.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            MIKE11EnergyLossClass.LossCoeffClass LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = LossCoeff_K;
            LossCoeff.LossNeg = LossCoeff_K;
            M11EnergyLoss.Userdefined = LossCoeff;
            return M11EnergyLoss;
        }

    }
}
