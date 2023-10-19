using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MIKE11BoundaryClass
    {
        public enum BoundaryTypes { WaterLevel, Discharge, QHLevel, PointSource, Distributed };
        public BoundaryTypes BoundaryType;
        public double Chainage;
        public string RiverName; 
        public MIKE11BoundaryClass(string riverName, double chainage)
            
        {
            RiverName = riverName;
            Chainage = chainage;   
        }
    }
}
