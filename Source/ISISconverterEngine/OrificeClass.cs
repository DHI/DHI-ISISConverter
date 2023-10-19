using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class OrificeClass: StructureClass
    {
        public double invert_zinv = 0;
        public double soffit_zsoff = 0;
        public double bore_area = 0;
        public double sill_up_zcup = 0;
        // level of sill on upstream side of structure (m AD)
        public double sill_down_zcdn = 0;
        // level of sill on downstream side of structure (m AD)
        public double Cweir = 1;
        // Discharge coefficient for weir flow over sill (typical value=1)
        public double Cfull = 1;
        // Discharge coefficient for surcharged flow (typical value=1)
        public double ModularLimit_m = 0.8;
        // Modular limit (eg 0.8)
        public bool flapped = false;
        public OrificeClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "ORIFICE";
            LineReaderClass l = new LineReaderClass();
            string stest = StArray[i].PadRight(StArray[i].Length + 1);
            int IndexOfFirstSpace = stest.IndexOf(" ");    
            if (IndexOfFirstSpace > -1) Keyword = StArray[i].Substring(0, IndexOfFirstSpace);
            if (Keyword == "INVERTED") Keyword = "INVERTED SYPHON";
            if (Keyword == "FLOOD") Keyword = "FLOOD RELIEF ARCH";
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            bool ok = true;
            stest = l.GetString(StArray[i], 1, ref ok);
            if (stest == "FLAPPED") flapped = true;
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            invert_zinv = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            soffit_zsoff = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
            bore_area  = l.GetDouble(StArray[i], 3, i, ref ok, ref errLineList);
            sill_up_zcup = l.GetDouble(StArray[i], 4, i, ref ok, ref errLineList);
            sill_down_zcdn = l.GetDouble(StArray[i], 5, i, ref ok, ref errLineList);
            i++;
            Cweir = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
            Cfull = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
            ModularLimit_m = l.GetDouble(StArray[i], 3, i, ref ok, ref errLineList);
        }
        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11CulvertClass M11Culvert = new MIKE11CulvertClass(lstructure);
            M11Culvert.Chainage = Chainage;
            M11Culvert.RiverName = RiverName;
            M11Culvert.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            M11Culvert.CulvertType = MIKE11CulvertClass.CulvertTypes.Rectangular;
            M11Culvert.Height = soffit_zsoff - invert_zinv;
            M11Culvert.Width = bore_area/M11Culvert.Height;
            M11Culvert.InvertLeveldown = invert_zinv;
            M11Culvert.InvertLevelup = invert_zinv;
            return M11Culvert;
        }
 


    }
}
