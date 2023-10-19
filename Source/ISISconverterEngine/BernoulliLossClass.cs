using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class BernoulliLossClass : StructureClass
    {
        private int NdataSets = 0;
        public class BernoulliLossDataClass
        {
            public double h = 0;
            public double AUp = 0;
            public double ADown = 0;
            public double KForward = 1.0;
            public double KBackward = 1.0;

        }
        public List<BernoulliLossDataClass> BernoulliLossData;
        public BernoulliLossClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "BERNOULLI";
            bool OK=true;    
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++; 
            NdataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            i++;
            BernoulliLossData = new List<BernoulliLossDataClass>();
            try
            {
                for (int ii = i; ii < i + NdataSets; ii++)
                {
                    BernoulliLossDataClass BernoulliData = new BernoulliLossDataClass();
                    BernoulliData.h = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList);
                    BernoulliData.AUp = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList);
                    BernoulliData.ADown = l.GetDouble(StArray[ii], 3, ii, ref OK, ref errLineList);
                    BernoulliData.KForward = l.GetDouble(StArray[ii], 4, ii, ref OK, ref errLineList);
                    BernoulliData.KBackward = l.GetDouble(StArray[ii], 5, ii, ref OK, ref errLineList);
                    BernoulliLossData.Add(BernoulliData);
                }
            }
            catch
            {
                List<int> lengthFormat = new List<int>() { 12, 12, 12, 10, 10 };
                for (int ii = i; ii < i + NdataSets; ii++)
                {
                    try
                    {
                        BernoulliLossDataClass BernoulliData = new BernoulliLossDataClass();
                        BernoulliData.h = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList, lengthFormat);
                        BernoulliData.AUp = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList, lengthFormat);
                        BernoulliData.ADown = l.GetDouble(StArray[ii], 3, ii, ref OK, ref errLineList, lengthFormat);
                        BernoulliData.KForward = l.GetDouble(StArray[ii], 4, ii, ref OK, ref errLineList, lengthFormat);
                        BernoulliData.KBackward = l.GetDouble(StArray[ii], 5, ii, ref OK, ref errLineList, lengthFormat);
                        BernoulliLossData.Add(BernoulliData);
                    }
                    catch (Exception e)
                    {
                        i = ii;
                        throw e;
                    }
                }
            }
            i = i + NdataSets-1;
        }
        double AverageKforward()
        { 
            
            double sum = 0;
            for (int ii = 0; ii < NdataSets; ii++) 
            {
                sum = BernoulliLossData[ii].KForward;
            }
            return sum / NdataSets;
        }
        double AverageKbackward()
        {

            double sum = 0;
            for (int ii = 0; ii < NdataSets; ii++)
            {
                sum = BernoulliLossData[ii].KBackward;
            }
            return sum / NdataSets;
        }

        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11EnergyLossClass M11EnergyLoss = new MIKE11EnergyLossClass(lstructure);
            M11EnergyLoss.Chainage = Chainage;
            M11EnergyLoss.RiverName = RiverName;
            M11EnergyLoss.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            MIKE11EnergyLossClass.LossCoeffClass LossCoeff = new MIKE11EnergyLossClass.LossCoeffClass();
            LossCoeff.LossPos = AverageKforward();
            LossCoeff.LossNeg = AverageKbackward();
            M11EnergyLoss.Userdefined = LossCoeff;
            return M11EnergyLoss;
        }
    }
}
