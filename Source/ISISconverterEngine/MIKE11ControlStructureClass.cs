using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    class MIKE11ControlStructureClass : MIKE11StructureClass
    {
       public enum ControlStrucTypes { Overflow, Underflow, Discharge, RadialGate, SluiceFormula}
       public ControlStrucTypes ControlStrucType = ControlStrucTypes.Overflow;
            public MIKE11HeadLossCoeffClass HeadLossFactorPositiveFlow;
            public MIKE11HeadLossCoeffClass HeadLossFactorNegativeFlow;
            public double GateWidth = 1;
            public double SillLevel = 0;
            public double Height = 1;
            public double Radius = 1;
            public double Trunnion = 1;
            public int NoGates = 1;
            public double UnderflowCc = 0.63;
            public MIKE11ControlStructureClass(StructureClass lstructure)
                : base(lstructure)
            {
                HeadLossFactorPositiveFlow = new MIKE11HeadLossCoeffClass();
                HeadLossFactorNegativeFlow = new MIKE11HeadLossCoeffClass();
            }
       
    }
}
