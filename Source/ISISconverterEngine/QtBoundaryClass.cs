using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class QtBoundaryClass
    {
        public String Keyword = "QTBDY";
        public string Comment = "";
        public LabelCollectionClass ID;
        private int NdataSets = 0;
        public double TimeLag = 0;
        public double TimeUnitInSeconds = 1;
        public double datumshift = 0;
        public double QMultiplier = 1;
        public double QMin = -1e10;
        public string TimeExtension = "";
        public class QtDataClass
        {
            public double Q;
            public double t;

        }
        public List<QtDataClass> QtCurve;
        public QtBoundaryClass(string[] StArray,ref int i, ref List<int> errLineList)
            {
            bool OK=true;    
            LineReaderClass l = new LineReaderClass();
                Comment = l.GetComment(Keyword, StArray[i]);
                i++;
                ID = new LabelCollectionClass(StArray[i]);
                
                 i++;
                 NdataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                 TimeLag = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                 if (!OK) TimeLag = 0; 
                 datumshift = l.GetDouble(StArray[i], 3, i, ref OK, ref errLineList);
                 if (!OK) datumshift=0;
                 TimeUnitInSeconds = l.GetTimeUnitinSec(StArray[i], 4);
                 TimeExtension = l.GetTimeExtensionMethod(StArray[i], 5, ref OK);
                 QMultiplier = l.GetDouble(StArray[i], 7, i, ref OK, ref errLineList);
                 if (!OK) QMultiplier = 1;
                 QMin = l.GetDouble(StArray[i], 8, i, ref OK, ref errLineList);
                 if (!OK) QMin = 0; ;
                 i++;
                 QtCurve = new List<QtDataClass>();
                 for(int ii=i;ii<i+NdataSets;ii++)
                     {
                        try
                        {
                             QtDataClass QtData = new QtDataClass();
                             QtData.Q = l.GetDouble(StArray[ii],1,ii,ref OK,ref errLineList);
                             QtData.t = l.GetDouble(StArray[ii],2,ii,ref OK,ref errLineList);
                     
                             QtCurve.Add(QtData);
                        }
                        catch(Exception e)
                        {
                            i = ii;
                            throw e;
                        }
                     }
                     i = i + NdataSets - 1; 
 
            }
        
        

    }
}
