using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MuskingumClass
    {
        public double deltaChainage;
        public string RiverName;
        public string Comment = "";
        public string Label = "";
        public double k;
        public double x;
        public MuskingumClass(ref int i, string[] filearray, ref double DeltaChain,  string Label)
        {
            string dummystring = filearray[i + 4];
            string kstring = dummystring.Substring(0, 10).Trim();
            this.k = System.Convert.ToDouble(kstring);
            string xstring = dummystring.Substring(10, 10).Trim();
            this.x = System.Convert.ToDouble(xstring);
            dummystring = filearray[i + 5].Trim();
            if (dummystring.Contains("VQ POWER LAW"))
            {
                i = i + 7;
            }
            else
                if (dummystring.Contains("VQ RATING"))
                {
                    dummystring = filearray[i + 6].Trim();
                    int n = System.Convert.ToInt32(dummystring);
                    i = i + 7+n;

                }



        }
        

    }
}
