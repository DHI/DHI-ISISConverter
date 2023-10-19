using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class QHBoundaryClass
    {
        public String Keyword = "QHBDY";
        public string Comment = "";
        public LabelCollectionClass ID;
        private int NdataSets = 0;
        public double datumshift = 0;
        
        public List<RatingCurveDataClass> RatingCurve;
        public QHBoundaryClass(string[] StArray,ref int i, ref List<int> errLineList)
        {
            bool OK=true;    
            LineReaderClass l = new LineReaderClass();
                Comment = l.GetComment(Keyword, StArray[i]);
                i++;
                ID = new LabelCollectionClass(StArray[i]);
                
                 i++;
                 NdataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                 datumshift = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            if (!OK) datumshift=0; 
                 i++;
                 RatingCurve = new List<RatingCurveDataClass>();
                 for(int ii=i;ii<i+NdataSets;ii++)
                     {
                        try
                        {
                            RatingCurveDataClass RatingData = new RatingCurveDataClass();
                            RatingData.Q = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList);
                            RatingData.h = l.GetDouble(StArray[ii], 2, ii, ref OK, ref errLineList);


                            RatingCurve.Add(RatingData);
                        }
                        catch (Exception e)
                        {
                            i = ii;
                            throw e;
                        }
                     }
                     i = i + NdataSets - 1; 
 
            }
        
        

    }
}
