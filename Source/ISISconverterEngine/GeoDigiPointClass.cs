using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
public    class GeoDigiPointClass
    {
        public double X; 
        public double Y;
        public string Label; 


        public GeoDigiPointClass(double GeoX, double GeoY, string label)
        {
            X = GeoX;
            Y = GeoY;
            Label = label;
        }

        public double Distance(GeoDigiPointClass digipoint2)
        {
            return Math.Sqrt((X - digipoint2.X) * (X - digipoint2.X) + (Y - digipoint2.Y) * (Y - digipoint2.Y));
        }
    }
}
