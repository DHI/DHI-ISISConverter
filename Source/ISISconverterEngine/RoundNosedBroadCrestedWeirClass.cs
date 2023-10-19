using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class RoundNosedBroadCrestedWeirClass : StructureClass
    {
        public double Cv, L, b, zc, m;
        public double p1,p2;
        public RoundNosedBroadCrestedWeirClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "RNWEIR";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            Cv = l.GetDouble(StArray[i],1, i, ref OK, ref errLineList);
            L = l.GetDouble(StArray[i],2, i, ref OK, ref errLineList);
            b = l.GetDouble(StArray[i],3, i, ref OK, ref errLineList);
            zc = l.GetDouble(StArray[i],4, i, ref OK, ref errLineList);
            m = l.GetDouble(StArray[i],5, i, ref OK, ref errLineList);
            i++;
            p1 = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            p2 = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
        }

        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11WeirClass M11Weir = new MIKE11WeirClass(lstructure);
            M11Weir.Chainage = Chainage;
            M11Weir.RiverName = RiverName;
            M11Weir.ID = Keyword +" "+ ID.Labels[0] +" "+ Comment;
            M11Weir.WeirType = MIKE11WeirClass.WeirTypes.WeirFormula1;
            M11Weir.width = b;
            M11Weir.Height = zc;
            M11Weir.InvertLevel = 0;
            return M11Weir;
        }
    }

}
