using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MIKE11PumpClass: MIKE11StructureClass
    {
        public enum SpecificationTypes { FixedDischarge, TabulatedCharacteristics }
        public SpecificationTypes SpecificationType = SpecificationTypes.FixedDischarge;
        public double Discharge = 0;
        public double StartLevel = -100;
        public double StopLevel = 100;
        public double StartUpPeriode = 0;
        public double CloseDownPeriod = 0;
        public MIKE11PumpClass(StructureClass lstructure)
            : base(lstructure)
        {
            
        }
    }
    
}
