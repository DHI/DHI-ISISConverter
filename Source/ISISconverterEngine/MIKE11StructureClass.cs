using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class MIKE11StructureClass
    {
        public string RiverName = "";
        public double Chainage = 5;
        public string ID = "";
        public bool sideStructure = false;
        public enum Valvesettings { none, OnlyPositive, OnlyNegative, NoFlow }
        public Valvesettings Valve = Valvesettings.none;
   

    public MIKE11StructureClass(StructureClass lstructure)
    {
        RiverName = lstructure.RiverName;
        Chainage = lstructure.Chainage;
        ID = lstructure.Comment;

    }
    }
}
