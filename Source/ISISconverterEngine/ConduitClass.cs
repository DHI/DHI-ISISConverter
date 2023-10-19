using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class ConduitdataPoint
    {
        public double x;
        public double y;
        public double k;
    }
    public class ConduitClass : SectionBaseClass
    {
        public enum ConduitTypes { Circular, FullArch, Section, Rectangular, SprungArch }
        public ConduitTypes CType = ConduitTypes.Circular;
        public enum FricTypes { Manningsn, ColebrookWhite };
        public FricTypes Fric = FricTypes.Manningsn;
        public String Keyword2 = "";
        public double invert = 0;
        public double diameter = 1;
        public double width = 1;
        public double height = 1;
        public double height2 = 1;
        public double fricnumber1 = 1;
        public double fricnumber2 = 1;
        public double fricnumber3 = 1;
        public List<ConduitdataPoint> DataPointCollection;
        public ConduitClass(string Keyword, string[] StArray, ref int i, ref List<int> errLineList)
            : base(Keyword, StArray, ref i, ref errLineList)
        {
            if (!(Keyword == "INTERPOLATE") && !(Keyword == "REPLICATE"))
            {
                i--;
                i--;
                bool OK = true;
                LineReaderClass l = new LineReaderClass();
                Keyword2 = l.GetString(StArray[i], 1, ref OK);
                if (Keyword2 == "RECTANGULA") Keyword2 = "RECTANGULAR";
                i++;
                i++;
                i++;
                switch (Keyword2)
                {
                    case "CIRCULAR":
                        {
                            string tstring = l.GetString(StArray[i], 1, ref OK);
                            if (tstring == "MANNING")
                            {
                                Fric = FricTypes.Manningsn;
                            }
                            else
                            {
                                Fric = FricTypes.ColebrookWhite;
                            }
                            i++;
                            invert = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            diameter = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            // bslot, dh, dslot, tslot, dh_top, hslot ignored 
                            i++;
                            fricnumber1 = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            fricnumber2 = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            break;
                        }
                    case "FULLARCH":
                        {
                            string tstring = l.GetString(StArray[i], 1, ref OK);
                            if (tstring == "MANNING")
                            {
                                Fric = FricTypes.Manningsn;
                            }
                            else
                            {
                                Fric = FricTypes.ColebrookWhite;
                            }
                            i++;
                            invert = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            width = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            height = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                            // bslot, dh, dslot, tslot, dh_top, hslot ignored 
                            i++;
                            fricnumber1 = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            fricnumber2 = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            break;
                        }
                    case "SECTION": // symmetrical
                        {
                            NumberOfPoints = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                            DataPointCollection = new List<ConduitdataPoint>();
                            Fric = FricTypes.ColebrookWhite;
                            i++;
                            for (int ii = i; ii < i + NumberOfPoints; ii++)
                            {
                                ConduitdataPoint data = new ConduitdataPoint();
                                data.x = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList);
                                data.y = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList);
                                data.k = l.GetDouble(StArray[ii], 3, ii, ref OK, ref errLineList);
                                DataPointCollection.Add(data);
                            }
                            i = i + NumberOfPoints - 1;
                            break;
                        }
                    case "RECTANGULAR":
                        {
                            string tstring = l.GetString(StArray[i], 1, ref OK);
                            if (tstring == "MANNING")
                            {
                                Fric = FricTypes.Manningsn;
                            }
                            else
                            {
                                Fric = FricTypes.ColebrookWhite;
                            }
                            i++;
                            invert = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            width = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            height = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                            // bslot, dh, dslot, tslot, dh_top, hslot ignored 
                            i++;
                            fricnumber1 = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            fricnumber2 = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            fricnumber3 = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                            break;
                        }
                    case "SPRUNG": goto case "SPRUNGARCH";
                    case "SPRUNGARCH":
                        {
                            string tstring = l.GetString(StArray[i], 1, ref OK);
                            if (tstring == "MANNING")
                            {
                                Fric = FricTypes.Manningsn;
                            }
                            else
                            {
                                Fric = FricTypes.ColebrookWhite;
                            }
                            i++;
                            invert = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            width = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            height = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                            height2 = l.GetDouble(StArray[i], 4, i, ref OK, ref errLineList);
                            // bslot, dh, dslot, tslot, dh_top, hslot ignored 
                            i++;
                            fricnumber1 = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                            fricnumber2 = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                            fricnumber3 = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                            break;
                        }
                }
            }
        }
        public ConduitClass(ConduitClass OriginalSection, double deltaChain, double deltaZ)
            : base(OriginalSection, deltaChain, deltaZ)
        
        {

            this.Keyword2 = OriginalSection.Keyword2;
            
            this.diameter = OriginalSection.diameter;
            this.Fric = OriginalSection.Fric;
            this.fricnumber1 = OriginalSection.fricnumber1;
            this.fricnumber2 = OriginalSection.fricnumber2;
            this.fricnumber3 = OriginalSection.fricnumber3;
            this.height = OriginalSection.height;
            this.height2 = OriginalSection.height2;
            this.invert = OriginalSection.invert - deltaZ;
            this.width = OriginalSection.width;
            this.NumberOfPoints = OriginalSection.NumberOfPoints;



            this.DataPointCollection = new List<ConduitdataPoint>();
            for (int i = 0; (i < NumberOfPoints); i++)
            {
                ConduitdataPoint lsurveydata = new ConduitdataPoint();
                lsurveydata.x = OriginalSection.DataPointCollection[i].x;
                lsurveydata.y = OriginalSection.DataPointCollection[i].y - deltaZ;
                lsurveydata.k = OriginalSection.DataPointCollection[i].k;
                DataPointCollection.Add(lsurveydata);
            }
            deltaZ = 0;
          
        }
    }

    

    
}

