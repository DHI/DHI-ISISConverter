using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class CrumpClass: StructureClass
    {
        public double BreadthCrest = 1;
        public double CallibrationCoeff = 1;
        public double ElevationCrest = 0;
        public double CrestHeightDownstream = 0;
        public double CrestHeightUpstream = 0;




        public CrumpClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "CRUMP";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            CallibrationCoeff = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            BreadthCrest = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            ElevationCrest = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            i++;
            CrestHeightUpstream = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            CrestHeightDownstream = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);

        }

        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11WeirClass M11Weir = new MIKE11WeirClass(lstructure);
            M11Weir.Chainage = Chainage;
            M11Weir.RiverName = RiverName;
            M11Weir.ID = "CRUMP"+ ID.Labels[0] + Comment;
            M11Weir.WeirType = MIKE11WeirClass.WeirTypes.WeirFormula1;
            M11Weir.width = BreadthCrest;
            M11Weir.Height = ElevationCrest;
            M11Weir.InvertLevel = 0;
            return M11Weir;
        }

        


    }
}
