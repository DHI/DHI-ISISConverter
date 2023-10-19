using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    class MIKE11TabulatedStructureClass : MIKE11StructureClass
    {
        public double[,] Q;
        public double[] Hup;
        public double[] Hdown;
        public enum TabulatedTypes { QHusHds, HusHdsQ, HdsHusQ }
        public TabulatedTypes TabulatedType = TabulatedTypes.QHusHds;
        public MIKE11TabulatedStructureClass(StructureClass lstructure)
            : base(lstructure)
        {
        }
    }
}
