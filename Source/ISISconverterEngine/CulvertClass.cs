using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class CulvertClass: StructureClass
    {
        public enum losstypes {inlet, bend, outlet};
        public losstypes  losstype = losstypes.inlet;
        public bool ReverseFlow = false;
        // Reverse Flow Mode; keyword ZERO (for zero headloss in reverse flow) 
        // or CALCULATED (for calculated head loss in reverse flow)
        public string HeadLossType = "TOTAL";
        // Keyword TOTAL to denote headloss based on total head, otherwise (keyword STATIC or blank) headloss is based on static head 
        public CulvertClass(string[] StArray, ref int i)
        {
            Keyword = "CULVERT";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            Keyword2 = l.GetString(StArray[i], 1, ref OK);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
        }
            
    }
}
