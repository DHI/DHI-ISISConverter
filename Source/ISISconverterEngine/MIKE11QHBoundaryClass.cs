using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MIKE11QHBoundaryClass: MIKE11BoundaryClass
    {
        public List<RatingCurveDataClass> RatingCurve;
         public MIKE11QHBoundaryClass(string riverName, double chainage, QHBoundaryClass QHBnd)
                : base(riverName, chainage)
            {
                BoundaryType = BoundaryTypes.QHLevel;
                RatingCurve = QHBnd.RatingCurve;
             // values are in Q - m3/s and WL - meters
         }

    }
}
