using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class CulvertInletClass : CulvertClass
    {
        public double UnsubmergedInletControlLoss_K = 0.0018;
        // Unsubmerged inlet control loss coefficient
        public double Exponent_M = 2.0;
        // Exponent of Flow Intensity for inlet control
        public double SubmergedInletControlLoss_c = 0.03;
        // Submerged inlet control loss coefficient
        public double SubmergedInletAdjustmentFactor_Y = 0.8;
        // Submerged inlet control adjustment factor
        public string ConduitType_e = "A";
        // Conduit type code; keyword ' for Type A, B for Type B
        public double OutletControlLoss_Ki = 0.5;
        // Outlet control loss coefficient
        public double TrashScreenWidth_Ws = 0;
        // Trash screen width (m)
        public double TrashScreen_r = 0;
        // Proportion of trash screen area occupied by bars (0 to 1.0)
        public double BlockageRatioTrash_b = 0;
        // Blockage ratio (proportion of trash screen area occupied by debris) (0 to 1.0)
        public double MaxTrashScreenHeight = 0; 
        // Max. Trash Screen Height
        public double TrashScreenHeadLossCoeff_Ks = 1.5;
        // Trash screen head loss coefficient, typical value is 1.5


        public CulvertInletClass(string[] StArray, ref int i, ref List<int> errLineList)
            : base(StArray, ref i)
        {
            LineReaderClass l = new LineReaderClass();
            bool ok = true;
            UnsubmergedInletControlLoss_K = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            Exponent_M = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
            SubmergedInletControlLoss_c = l.GetDouble(StArray[i], 3, i, ref ok, ref errLineList);
            SubmergedInletAdjustmentFactor_Y = l.GetDouble(StArray[i], 4, i, ref ok, ref errLineList);
            OutletControlLoss_Ki = l.GetDouble(StArray[i], 5, i, ref ok, ref errLineList);
            ConduitType_e = l.GetString(StArray[i], 6, ref ok);
            i++;
            TrashScreenWidth_Ws = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            TrashScreen_r = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
            BlockageRatioTrash_b = l.GetDouble(StArray[i], 3, i, ref ok, ref errLineList);
            TrashScreenHeadLossCoeff_Ks = l.GetDouble(StArray[i], 4, i, ref ok, ref errLineList);
            string stest = l.GetString(StArray[i], 5, ref ok);
            if (stest == "CALCULATED") ReverseFlow = true;
            stest = l.GetString(StArray[i], 6, ref ok);
            if (stest != "TOTAL") HeadLossType = "ZERO";
            MaxTrashScreenHeight = l.GetDouble(StArray[i], 7, i, ref ok, ref errLineList);
        }

        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11EnergyLossClass M11EnergyLoss = new MIKE11EnergyLossClass(lstructure);
            M11EnergyLoss.Chainage = Chainage;
            M11EnergyLoss.RiverName = RiverName;
            M11EnergyLoss.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            MIKE11EnergyLossClass.LossCoeffClass LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = 0.5;
            LossCoeff.LossNeg = 0.5;
            M11EnergyLoss.Contraction = LossCoeff;
            return M11EnergyLoss;
        }
  

 

 
 


    }
}
