using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class SyphonClass : StructureClass
    {
        public double zc, zsoff, Area, zmax;
        public double CWeir, Cfull, m, zprime;
        public SyphonClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "SYPHON";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            zc = l.GetDouble(StArray[i],1, i, ref OK, ref errLineList);
            zsoff = l.GetDouble(StArray[i],2, i, ref OK, ref errLineList);
            Area = l.GetDouble(StArray[i],3, i, ref OK, ref errLineList);
            zmax = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
            i++;
            CWeir = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            Cfull = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            m = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
            zprime = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11ControlStructureClass M11Controlstruc = new MIKE11ControlStructureClass(lstructure);
            M11Controlstruc.Chainage = Chainage;
            M11Controlstruc.RiverName = RiverName;
            M11Controlstruc.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            M11Controlstruc.ControlStrucType = MIKE11ControlStructureClass.ControlStrucTypes.Underflow;

            double Height = zmax - zc;
            M11Controlstruc.GateWidth = Area /Height;
            M11Controlstruc.SillLevel = zc;
            M11Controlstruc.UnderflowCc = Cfull;
            return M11Controlstruc;
        }
    }
}
