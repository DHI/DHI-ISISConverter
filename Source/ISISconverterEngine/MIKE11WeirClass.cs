using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    
    public class MIKE11WeirClass: MIKE11StructureClass
    {
        public enum WeirTypes { BroadCrestedWeir, WeirFormula1 }
        public WeirTypes WeirType = WeirTypes.WeirFormula1;
        public MIKE11HeadLossCoeffClass HeadLossFactorPositiveFlow;
        public MIKE11HeadLossCoeffClass HeadLossFactorNegativeFlow;
        public double width = 1;
        public double Height = 1;
        public double WeirCoeff = 1.838;
        public double WeirExp = 1.5;
        public double InvertLevel = 1;
        public MIKE11WeirClass(StructureClass lstructure)
            : base(lstructure)
        {
            HeadLossFactorPositiveFlow = new MIKE11HeadLossCoeffClass();
            HeadLossFactorNegativeFlow = new MIKE11HeadLossCoeffClass();
        }
    }
}
