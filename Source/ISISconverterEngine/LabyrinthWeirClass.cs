using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class LabyrinthWeirClass : StructureClass
    {
        public double P1, alpha, W, N, B, A, t, zc, L;
        public double ccf, m, cdlim, L1, D; 
        public string shflag;
        public LabyrinthWeirClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "LABYRINTH WEIR";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            P1 = l.GetDouble(StArray[i],1, i, ref OK, ref errLineList);
            alpha = l.GetDouble(StArray[i],2, i, ref OK, ref errLineList);
            W = l.GetDouble(StArray[i],3, i, ref OK, ref errLineList);
            N = l.GetDouble(StArray[i],4, i, ref OK, ref errLineList);
            B = l.GetDouble(StArray[i],5, i, ref OK, ref errLineList);
            A = l.GetDouble(StArray[i], 6, i, ref OK, ref errLineList);
            t = l.GetDouble(StArray[i], 7, i, ref OK, ref errLineList);
            zc = l.GetDouble(StArray[i], 8, i, ref OK, ref errLineList);
            L = l.GetDouble(StArray[i], 9, i, ref OK, ref errLineList);
            i++;
            ccf = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            m = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            cdlim = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            L1 = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
            D = l.GetDouble(StArray[i], 5, i, ref OK, ref errLineList);
            if (OK == false) D = 0;
            OK = true;
            shflag = l.GetString(StArray[i], 6, ref OK);
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11WeirClass M11Weir = new MIKE11WeirClass(lstructure);
            M11Weir.Chainage = Chainage;
            M11Weir.RiverName = RiverName;
            M11Weir.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            M11Weir.WeirType = MIKE11WeirClass.WeirTypes.WeirFormula1;
            M11Weir.width = L;
            M11Weir.Height = zc;
            M11Weir.InvertLevel = 0;
            return M11Weir;
        }
    }
}
