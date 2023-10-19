using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class PondClass : StructureClass
    {
        public string Keyword3 = "OUTFLOW";
        public int NoStageArea = 0;

        public class ElevationAreaDataSetClass
        {
            public double elevation = 0;
            public double area = 0;
        }
        public ElevationAreaDataSetClass[] StageAreaCurve;

        public class PondWeirClass
        {
            public double Cdw = 0.59; 
            // weir discharge coefficient
            public double Bw = 0; 
            // width across flow (m)
            public double ZCw = 0; 
            //weir crest elevation (mAD)
        }
        public List<PondWeirClass> WeirCollection;

        public class PondSluiceClass
        {
            
            public double Cds = 0.6; 
            // sluice discharge coefficient
            public double Asluice = 0;
            // area of opening in sluice (m2)
            public double zcs = 0;
            // sluice invert level (mAD)
            public double ds = 0;
            // depth of sluice opening (m)
        }
        public List<PondSluiceClass> SluiceCollection;

        public class DischargeElevationDataSetClass
        {
            public double Q = 0;
            public double h = 0;
        }
        public class RatingCurveClass
        {
            public int NoRatingcurvepoints = 0;
            public DischargeElevationDataSetClass[] RatingCurve;
        }
        public List<RatingCurveClass> RatingCurveCollection;


        public PondClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "POND";
            Keyword2 = "ONLINE";
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            bool ok = true;
            Keyword2 = l.GetString(StArray[i], 1, ref ok);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            NoStageArea = l.GetInt(StArray[i], 1, i, ref ok,ref errLineList);
            StageAreaCurve = new ElevationAreaDataSetClass[NoStageArea];
            i++;
            int index = 0;
            for (int ii = i; (ii < i + NoStageArea); ii++)
            {
                try
                {
                    StageAreaCurve[index].elevation = l.GetDouble(StArray[ii], 1, ii, ref ok, ref errLineList);
                    StageAreaCurve[index].area = l.GetDouble(StArray[ii], 2, ii, ref ok, ref errLineList);
                    index++;
                }
                catch(Exception e)
                {
                    i = ii;
                    throw e;
                }
            }
            i = i + NoStageArea;
            i++;  // nstruc =1;
            i++;  // OUTFLOW
            i++;
            int  noutflow = l.GetInt(StArray[i], 1, i, ref ok, ref errLineList);
            WeirCollection = new List<PondWeirClass>();
            SluiceCollection = new List<PondSluiceClass>();
            RatingCurveCollection = new List<RatingCurveClass>();

            for (int ii = 0; (ii < noutflow); ii++)
            {
                i++;
                string Keyword4 = (StArray[i].Trim()).ToUpper();
                
                switch (Keyword4)
                {
                    case "OUTFLOW WEIR":
                        {
                            i++;
                            PondWeirClass lweir = new PondWeirClass();
                            lweir.Cdw = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
                            lweir.Bw = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
                            lweir.ZCw = l.GetDouble(StArray[i], 3, i, ref ok, ref errLineList);
                            WeirCollection.Add(lweir);
                            break;
                        }
                    case "OUTFLOW SLUICE":
                        {
                            i++;
                            PondSluiceClass lsluice = new PondSluiceClass();
                            lsluice.Cds = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
                            lsluice.Asluice= l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
                            lsluice.zcs = l.GetDouble(StArray[i], 3, i, ref ok, ref errLineList);
                            lsluice.ds = l.GetDouble(StArray[i], 4, i, ref ok, ref errLineList);
                            SluiceCollection.Add(lsluice);
                            break;
                        }
                    case "OUTFLOW RAITING":
                        {
                            i++;
                            RatingCurveClass lRatingcurve = new RatingCurveClass();
                            lRatingcurve.NoRatingcurvepoints = l.GetInt(StArray[i],1, i, ref ok, ref errLineList);
                            lRatingcurve.RatingCurve = new DischargeElevationDataSetClass[lRatingcurve.NoRatingcurvepoints];
                            for (int iii = 0; (iii < lRatingcurve.NoRatingcurvepoints); iii++)
                            {
                                i++;
                                DischargeElevationDataSetClass lQh = new DischargeElevationDataSetClass();
                                lQh.Q = l.GetDouble(StArray[i], 1, i, ref ok, ref errLineList);
                                lQh.h = l.GetDouble(StArray[i], 2, i, ref ok, ref errLineList);
                                lRatingcurve.RatingCurve[iii] = lQh;
                            }

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

        }

    }
}
