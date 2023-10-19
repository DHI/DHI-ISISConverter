using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class CrossSectionClass : SectionBaseClass
    {
        public class SurveydataClass
        {
            public double x=0, z=0, n=0, GeoX=0, GeoY=0;
        }

        public SurveydataClass[] Surveydata;
        public int marker1=-1, marker2=-1, marker3=-1;
        


        public CrossSectionClass(CrossSectionClass CopyXsec): base(CopyXsec)
    {
        NumberOfPoints = CopyXsec.NumberOfPoints;
            this.marker1 = CopyXsec.marker1;
            this.marker2 = CopyXsec.marker2;
            this.marker3 = CopyXsec.marker3;
            this.Surveydata = new SurveydataClass[NumberOfPoints];
            for (int i = 0; (i < NumberOfPoints); i++)
            {
                SurveydataClass lSurveydata = new SurveydataClass();
                lSurveydata.x = CopyXsec.Surveydata[i].x;
                lSurveydata.z = CopyXsec.Surveydata[i].z;
                lSurveydata.n = CopyXsec.Surveydata[i].n;
                this.Surveydata[i] = lSurveydata;
            }
            

    }

        public CrossSectionClass(string Keyword, string[] StArray, ref int i, ref List<int> errLineList) : base(Keyword, StArray, ref i, ref errLineList)
        {


            if (!(Keyword == "INTERPOLATE") && !(Keyword == "REPLICATE"))
            {
                bool OK = false;
                i++;

                LineReaderClass l = new LineReaderClass();
                if (Keyword == "RIVER")
                {
                    NumberOfPoints = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                }
                else
                    NumberOfPoints = 0;
                i++;
                Surveydata = new SurveydataClass[NumberOfPoints];
                for (int ii = 0; (ii < NumberOfPoints); ii++)
                {
                    try
                    {
                        AddData(StArray[i + ii], ii, i+ii, ref errLineList);
                    }
                    catch(Exception e)
                    {
                        i += ii;
                        throw e;
                    }
                }
                i = i + NumberOfPoints - 1;
            }
        }

        public CrossSectionClass()
            : base()
        {

                NumberOfPoints = 3;
               
                
                Surveydata = new SurveydataClass[NumberOfPoints];
                for (int ii = 0; (ii < NumberOfPoints); ii++)
                {
                    SurveydataClass lsurveydata = new SurveydataClass();
                    lsurveydata.x = -1+ii;
                    lsurveydata.z = 1-(ii % 2);
                    lsurveydata.n = 0.025;
                    Surveydata[ii] = lsurveydata;
                }
                
            
        }

        public CrossSectionClass(CrossSectionClass OriginalSection, double deltaChain, double deltaZ)
            : base(OriginalSection, deltaChain, deltaZ)
        
        {
            Surveydata = new SurveydataClass[NumberOfPoints];
            for (int i = 0; (i < NumberOfPoints); i++)
            {
                SurveydataClass lsurveydata = new SurveydataClass();
                lsurveydata.x = OriginalSection.Surveydata[i].x;
                lsurveydata.z = OriginalSection.Surveydata[i].z - deltaZ;
                lsurveydata.n = OriginalSection.Surveydata[i].n;
                Surveydata[i] = lsurveydata;
            }
            deltaZ = 0;
          
        }


        private void AddData(string Line,int index, int lineNum, ref List<int> errLineList)
        {
            System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo(); 
            info.NumberDecimalSeparator = "."; info.NumberGroupSeparator = ",";

            LineReaderClass l = new LineReaderClass();
            bool OK = false;

            Surveydata[index] = new SurveydataClass();
            string dummystring;
            dummystring = Line.Substring(0, 10);
            dummystring = dummystring.Trim();
            Surveydata[index].x = l.GetDouble(dummystring, 1, lineNum,ref OK, ref errLineList);
            dummystring = Line.Substring(10, 10);
            dummystring = dummystring.Trim();
            Surveydata[index].z = l.GetDouble(dummystring, 1, lineNum, ref OK, ref errLineList);
            dummystring = Line.Substring(20, Math.Min(10, Line.Length - 20));
            dummystring = dummystring.Trim();
            Surveydata[index].n = l.GetDouble(dummystring, 1, lineNum, ref OK, ref errLineList);
            if (Line.Length > 40)
            {
                dummystring = Line.Substring(40, Math.Min(10,Line.Length-40));
                dummystring = (dummystring.Trim()).ToUpper();
      
                switch (dummystring)
                {
                    case "LEFT":
                        marker1 = index;
                        break;
                    case "RIGHT":
                        marker3 = index;
                        break;
                    case "BED":
                        marker2 = index;
                        break;
                    default:
                        break;
                }
            }
            if (Line.Length > 50)
            {
                dummystring = Line.Substring(50, 10);
                dummystring = dummystring.Trim();
                Surveydata[index].GeoX = l.GetDouble(dummystring, 1, lineNum, ref OK, ref errLineList);
                dummystring = Line.Substring(60, 10);
                dummystring = dummystring.Trim();
                Surveydata[index].GeoY = l.GetDouble(dummystring, 1, lineNum, ref OK, ref errLineList);
            }
        }
    }
}
