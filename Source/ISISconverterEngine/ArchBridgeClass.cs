using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    class ArchBridgeClass: BridgeClass
    {
         
        public ArchBridgeClass(string[] StArray, ref int i, ref List<int> errLineList)
            : base(StArray, ref i, ref errLineList)
        {
            i++;
            ReadRawData(StArray, ref i, ref errLineList);
            ReadArchData(StArray, ref i, ref errLineList);
        }
        

            
    }
}
