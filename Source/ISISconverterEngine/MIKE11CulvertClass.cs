using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    
    
        
        public class MIKE11CulvertClass : MIKE11StructureClass
        {
            public enum CulvertTypes { Rectangular, Circular, IrregularLevelWidth, IrregularLeveldepth }
            public CulvertTypes CulvertType = CulvertTypes.Rectangular;
            public MIKE11HeadLossCoeffClass HeadLossFactorPositiveFlow;
            public MIKE11HeadLossCoeffClass HeadLossFactorNegativeFlow;
            public double Width = 1;
            public double Height = 1;
            public double Diameter = 1;
            public double Length = 1;
            public double InvertLevelup = 1;
            public double InvertLeveldown = 1;
            public int NoCulverts = 1;
            public double Manningsn = 0.013;
            public MIKE11CulvertClass(StructureClass lstructure)
                : base(lstructure)
            {
                HeadLossFactorPositiveFlow = new MIKE11HeadLossCoeffClass();
                HeadLossFactorNegativeFlow = new MIKE11HeadLossCoeffClass();
            }
            
        }
    
}
