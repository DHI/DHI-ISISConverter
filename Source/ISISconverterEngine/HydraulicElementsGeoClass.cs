using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class HydraulicElementsGeoClass
    {
        public enum ElementTypes { ABSTRACTION, BERNOULLI_LOSS, BLOCKAGE, QHBDY, QTBDY, HTBDY, TIDBDY, BRIDGE_ARCH, BRIDGE_USBPR1978,
        CONDUIT_CIRCULAR, CONDUIT_FULLARCH, CONDUIT_SECTION, CONDUIT_RECTANGULAR, CONDUIT_SPRUNGARCH, CONDUIT_SPRUNG,
        CULVERT_INLET, CULVERT_OUTLET, CULVERT_BEND, LOSS,
        FSSR16BDY, SCSBDY, INTERPOLATE, JUNCTION_OPEN,
        ORIFICE_OPEN, ORIFICE_FLAPPED, INVERTED_SYPHON_OPEN, INVERTED_SYPHON_FLAPPED, MANHOLE, OUTFALL_OPEN, OUTFALL_FLAPPED,
        FLOOD_RELIEF_ARCH_OPEN, FLOOD_RELIEF_ARCH_FLAPPED, POND,LABYRINTH,
        OCPUMP, QRATING, REPLICATE, RESERVOIR, RIVER_SECTION, RIVER_MUSKINGUM, RIVER_MUSK_VPMC, RIVER_MUSK_XSEC, RULES,
        SLUICE_RADIAL, SLUICE_VERTICAL, SPILL, CRUMP, GATED_WEIR, NOTWEIR, QH_CONTROL, RNWEIR, SCWEIR, SYPHON, WEIR, LATERAL,FEHBDY, FLOODPLAIN_SECTION, NOTDEFINED
        };

        public ElementTypes Element;
        public string Label = "";
        public string IDPoint;
        public double GeoX = 0;
        public double GeoY = 0;
        public  List<HydraulicElementsGeoClass> ConnectionList;


        public static int CompareElements(HydraulicElementsGeoClass element1, HydraulicElementsGeoClass element2)
        {
            if (element1.IDPoint == null)
            {
                if (element2.IDPoint == null)
                {
                    // If element1 is null and element2 is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If element1 is null and element2 is not null, 
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If element1 is not null...
                //
                if (element2 == null)
                // ...and element2 is null, element1 is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and element2 is not null, compare the 
                    // the two strings.
                    //
                    
                        return element1.IDPoint.CompareTo(element2.IDPoint);
                    }
                }
            }
        

        private ElementTypes getEnumerator(string KeyWord)
        {
            switch (KeyWord)
            {
                case "ABSTRACTION_":
                    return ElementTypes.ABSTRACTION;
                case "BERNOULLI_":
                    return ElementTypes.BERNOULLI_LOSS;
                case "QHBDY_":
                    return ElementTypes.QHBDY;
                case "QTBDY_":
                    return ElementTypes.QTBDY;
                case "HTBDY_":
                    return ElementTypes.HTBDY;
                case "TIDBDY_":
                    return ElementTypes.TIDBDY;
                case "BRIDGE_ARCH":
                    return ElementTypes.BRIDGE_ARCH;
                case "BRIDGE_USBPR1978":
                    return ElementTypes.BRIDGE_USBPR1978;
                case "CONDUIT_CIRCULAR":
                    return ElementTypes.CONDUIT_CIRCULAR;
                case "CONDUIT_FULLARCH":
                    return ElementTypes.CONDUIT_FULLARCH;
                case "CONDUIT_SECTION":
                    return ElementTypes.CONDUIT_SECTION;
                case "CONDUIT_RECTANGULAR":
                    return ElementTypes.CONDUIT_RECTANGULAR;
                case "CONDUIT_SPRUNGARCH":
                    return ElementTypes.CONDUIT_SPRUNGARCH;
                case "CONDUIT_SPRUNG":
                    return ElementTypes.CONDUIT_SPRUNG;
                case "CULVERT_INLET":
                    return ElementTypes.CULVERT_INLET;
                case "CULVERT_OUTLET":
                    return ElementTypes.CULVERT_OUTLET;
                case "CULVERT_BEND":
                    return ElementTypes.CULVERT_BEND;
                case "FSSR16BDY_":
                    return ElementTypes.FSSR16BDY;
                case "SCSBDY_":
                    return ElementTypes.SCSBDY;
                case "INTERPOLATE_":
                    return ElementTypes.INTERPOLATE;
                case "JUNCTION_OPEN":
                    return ElementTypes.JUNCTION_OPEN;
                case "LOSS_":
                    return ElementTypes.LOSS;
                case "ORIFICE_OPEN":
                    return ElementTypes.ORIFICE_OPEN;
                case "ORIFICE_FLAPPED":
                    return ElementTypes.ORIFICE_FLAPPED;
                case "INVERTED_SYPHON-OPEN":
                    return ElementTypes.INVERTED_SYPHON_OPEN;
                case "INVERTED_SYPHON-FLAPPED":
                    return ElementTypes.INVERTED_SYPHON_FLAPPED;
                case "MANHOLE_":
                    return ElementTypes.MANHOLE;
                case "OUTFALL_OPEN":
                    return ElementTypes.OUTFALL_OPEN;
                case "OUTFALL_FLAPPED":
                    return ElementTypes.OUTFALL_FLAPPED;
                case "FLOOD_RELIEF-ARCH-OPEN":
                    return ElementTypes.FLOOD_RELIEF_ARCH_OPEN;
                case "FLOOD_RELIEF-ARCH-FLAPPED":
                    return ElementTypes.FLOOD_RELIEF_ARCH_FLAPPED;
                case "OCPUMP_":
                    return ElementTypes.OCPUMP;
                case "QRATING_":
                    return ElementTypes.QRATING;
                case "REPLICATE_":
                    return ElementTypes.REPLICATE;
                case "RESERVOIR_":
                    return ElementTypes.RESERVOIR;
                case "RIVER_SECTION":
                    return ElementTypes.RIVER_SECTION;
                case "RIVER_MUSKINGUM":
                    return ElementTypes.RIVER_MUSKINGUM;
                case "RIVER_MUSK-VPMC":
                    return ElementTypes.RIVER_MUSK_VPMC;
                case "RIVER_MUSK-XSEC":
                    return ElementTypes.RIVER_MUSK_XSEC;
                case "RULES_":
                    return ElementTypes.RULES;
                case "SLUICE_RADIAL":
                    return ElementTypes.SLUICE_RADIAL;
                case "SLUICE_VERTICAL":
                    return ElementTypes.SLUICE_VERTICAL;
                case "SPILL_":
                    return ElementTypes.SPILL;
                case "CRUMP_":
                    return ElementTypes.CRUMP;
                case "GATED WEIR_":
                    return ElementTypes.GATED_WEIR;
                case "NOTWEIR_":
                    return ElementTypes.NOTWEIR;
                case "QH_CONTROL":
                    return ElementTypes.QH_CONTROL;
                case "RNWEIR_":
                    return ElementTypes.RNWEIR;
                case "SCWEIR_":
                    return ElementTypes.SCWEIR;
                case "SYPHON_":
                    return ElementTypes.SYPHON;
                case "WEIR_":
                    return ElementTypes.WEIR;
                case "LATERAL_":
                    return ElementTypes.LATERAL;
                case "FEHBDY_":
                    return ElementTypes.FEHBDY;
                case "FLOODPLAIN_SECTION":
                    return ElementTypes.FLOODPLAIN_SECTION;
                    
                default:
                    return ElementTypes.NOTDEFINED;
            }
        }

        public HydraulicElementsGeoClass(string[] GeoString)
        {
            System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo();
            info.NumberDecimalSeparator = "."; info.NumberGroupSeparator = ",";

            string identify = GeoString[0];
            int IndexOfParanthesis =identify.IndexOf("[");
            if (IndexOfParanthesis > -1)
            {
                int FirstUnderscore = identify.IndexOf("_");
                if (FirstUnderscore > -1)
                {
                    int SecondUnderscore = identify.IndexOf("_", FirstUnderscore + 1);
                    if (SecondUnderscore > -1)
                    {
                        string KeyWord = identify.Substring(1, SecondUnderscore-1);
                        Element = getEnumerator(KeyWord);
                        Label = identify.Substring(SecondUnderscore+1, identify.Length - SecondUnderscore-2);
                        IDPoint = identify.Substring(1, identify.Length - 2);
                        string xstring = GeoString[1].Substring(2);
                        GeoX = System.Convert.ToDouble(xstring, info);
                        string ystring = GeoString[2].Substring(2);
                        GeoY = System.Convert.ToDouble(ystring, info);
                        ConnectionList = new List<HydraulicElementsGeoClass>();

                    }
                }
            }

        }

    }
}
