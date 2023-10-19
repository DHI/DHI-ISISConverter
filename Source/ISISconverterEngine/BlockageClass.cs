using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class BlockageClass: StructureClass
    {
        public double KInlet = 0;
        public double KOutlet = 0;
        private int NdataSets = 0;
        public double TLag = 0;
        public double TimeUnitInSeconds = 1;
        public string TimeExtension = "";
        public class BlockageDataClass
        {
            public double t = 0;
            public double p = 0;

        }
        public List<BlockageDataClass> BlockageData;
        public BlockageClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "BLOCKAGE";
            bool OK=true;    
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++; 
            KInlet = l.GetDouble(StArray[i],1, i, ref OK, ref errLineList);
            KOutlet = l.GetDouble(StArray[i],2, i, ref OK, ref errLineList);
            i++; 
            NdataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            TLag = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            TimeUnitInSeconds = l.GetTimeUnitinSec(StArray[i], 3);
            TimeExtension = l.GetTimeExtensionMethod(StArray[i], 4, ref OK);
            i++;
            BlockageData = new List<BlockageDataClass>();
            for (int ii = i; ii < i+NdataSets; ii++)
            {
                try
                {
                    BlockageDataClass LBlockageData = new BlockageDataClass();
                    LBlockageData.t = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList);
                    LBlockageData.p = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList);
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NdataSets-1;
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11EnergyLossClass M11EnergyLoss = new MIKE11EnergyLossClass(lstructure);
            M11EnergyLoss.Chainage = Chainage;
            M11EnergyLoss.RiverName = RiverName;
            M11EnergyLoss.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            MIKE11EnergyLossClass.LossCoeffClass LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = KInlet;
            LossCoeff.LossNeg = KInlet;
            M11EnergyLoss.Contraction = LossCoeff;
            LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = KOutlet;
            LossCoeff.LossNeg = KOutlet;
            M11EnergyLoss.Expansion = LossCoeff;
            return M11EnergyLoss;
        }
    }
}
