using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class SectionBaseClass
    {
        public double deltaChainage = 0;
        public bool dummy = false;
        public List<string> Label;
        public string Comment = "";
        public bool Interpolate = false;
        public bool copy = false;
        public int NumberOfPoints = 0;
        public double deltaZ = 0;
        public string Keyword;

        public SectionBaseClass(SectionBaseClass CopyXsec)
    {
        NumberOfPoints = CopyXsec.NumberOfPoints;
        Comment = "Copy of" + CopyXsec.Label[0];

    }
        public SectionBaseClass(SectionBaseClass OriginalSection, double deltaChain, double deltaZ)
        {
            Keyword = OriginalSection.Keyword;
            NumberOfPoints = OriginalSection.NumberOfPoints;
            this.deltaZ = deltaZ;
            Label = new List<string>();
            deltaChainage = deltaChain;

        }
        public SectionBaseClass()
        {
            Keyword = "RIVER";
            this.deltaZ = 0;
            Label = new List<string>();
            deltaChainage = 0;
            dummy = true;
        }

        public void GetLabels(string labelsstring)
        {
            int ii =1;
            string stest = "NOTAssigned";
            LineReaderClass l = new LineReaderClass();
            while (!(stest == ""))
            {
                stest = l.GetLabel(labelsstring, ii);
                if (!(stest == ""))
                {
                    Label.Add(stest);
                }
                ii++;
            }
        }

        public SectionBaseClass(string Keyword, string[] StArray, ref int i, ref List<int> errLineList)
        {
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            this.Keyword = Keyword;
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            switch(Keyword)
         
            {
                
                case "CONDUIT":
                {
                    i++;
                    
                    break;
                }
                case "INTERPOLATE":
                {
                    break;
                }
                case "RIVER":
                {
                    i++;
                    break;
                }
        }
            Label = new List<string>();
            GetLabels(StArray[i]);
            i++;
            deltaChainage = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);

            
        }
    }
}
