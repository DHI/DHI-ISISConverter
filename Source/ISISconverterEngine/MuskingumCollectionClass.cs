using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MuskingumCollectionClass
    {
        string Label="";
        
        List<MuskingumClass> MuskingumList;
        public MuskingumCollectionClass()
        {
            MuskingumList = new List<MuskingumClass>();
           
        }
        public int add(int i, string[] filearray, ref double DeltaChain)
        {
             
            
            string Comment = filearray[i].Trim().Substring(5).Trim();
            string dummystring = filearray[i + 2].PadRight(filearray[i + 2].Length + 1);
            Label = dummystring.Substring(0,dummystring.IndexOf(" "));
            dummystring = filearray[i + 3];
            
            if (dummystring.Length > 9)
              dummystring = dummystring.Substring(0, 10);
            dummystring = dummystring.Trim();
            double deltaChainage = Convert.ToDouble(dummystring);

            int ii = i;
            MuskingumClass MuskingumElement = new MuskingumClass(ref ii, filearray, ref deltaChainage, Label);
            MuskingumList.Add(MuskingumElement);
            DeltaChain = deltaChainage;
            return ii;
            
        }
    }
}
