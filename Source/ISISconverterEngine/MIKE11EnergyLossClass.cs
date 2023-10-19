using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MIKE11EnergyLossClass :  MIKE11StructureClass
    {
        
        public class LossCoeffClass

        {
            public double LossPos = 0;
            public double LossNeg = 0;
        }
        public LossCoeffClass Userdefined;
        public LossCoeffClass Contraction;
        public LossCoeffClass Expansion;
        public MIKE11EnergyLossClass(StructureClass lstructure)
            : base(lstructure)
        {
           
        }
    }
   
}
