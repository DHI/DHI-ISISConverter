using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{

    public class LinkGeometryPairClass
    {
        public static int Compare(LinkGeometryPairClass x, LinkGeometryPairClass y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    //  two depth.
                    //
                    if (x.Depth > y.Depth) return 1;
                    if (x.Depth < y.Depth) return -1;
                    return 0;

                }
            }
        }


        public double Depth = 0;
        public double Width = 0;
    }
    public class MIKE11LinkChannelReachClass : RiverReachClass
    {

        private double DeltaWidth(double x1, double y1, double x2, double y2, double WaterLevel)
        {
            double dWidth = 0;
            if (y1 > WaterLevel)
            {
                if (y2 > WaterLevel)
                    dWidth = 0;
                else // intersection
                {
                    double Xintersection = x1 + (WaterLevel - y1) / (y2 - y1) * (x2 - x1) + x1;
                    dWidth = x2 - Xintersection;
                }

            }
            else
                if (y2 < WaterLevel)
                    dWidth = x2 - x1;
                else // intersection
                {
                    if (y1 == y2)
                        dWidth = x2 - x1;
                    else
                    {
                        double Xintersection = x1 + (WaterLevel - y1) / (y2 - y1) * (x2 - x1) + x1;
                        dWidth = Xintersection - x1;
                    }
                }


            return dWidth;
        }

        private double Width(double WL, SpillClass.SurveyDataClass[] Surveydata)
        {
            double dW = 0;
            for (int i = 1; i < Surveydata.Count(); i++)
            {
                dW = dW + DeltaWidth(Surveydata[i - 1].x, Surveydata[i - 1].z, Surveydata[i].x, Surveydata[i].z, WL);
            }
            return dW;
        }
        public double BedLevelUs = 0;
        public double BedLevelDs = 0;
        public List<LinkGeometryPairClass> DepthWidthCollection;

        public MIKE11LinkChannelReachClass(string RiverName, SpillClass.SurveyDataClass[] Surveydata)
            : base(RiverName)
        {
            BranchType = BranchTypes.Link;
            double Maxz = Surveydata[0].z;
            double Minz = Surveydata[0].z;
            LinkGeometryPairClass lLinkGpair;
            DepthWidthCollection = new List<LinkGeometryPairClass>();
            for (int i = 0; i < Surveydata.Count(); i++)
            {
                Maxz = Math.Max(Maxz, Surveydata[i].z);
                Minz = Math.Min(Minz, Surveydata[i].z);
                lLinkGpair = new LinkGeometryPairClass();
                lLinkGpair.Depth = Surveydata[i].z;
                lLinkGpair.Width = Width(Surveydata[i].z, Surveydata);
                DepthWidthCollection.Add(lLinkGpair);
            }
            lLinkGpair = new LinkGeometryPairClass();
            lLinkGpair.Depth = Maxz + 2;
            lLinkGpair.Width = Width(lLinkGpair.Depth, Surveydata);
            DepthWidthCollection.Add(lLinkGpair);
            DepthWidthCollection.Sort(LinkGeometryPairClass.Compare);
            BedLevelUs = Minz;
            BedLevelDs = Maxz;
            for (int i = 0; i < DepthWidthCollection.Count(); i++)
            {
                DepthWidthCollection[i].Depth = DepthWidthCollection[i].Depth - Minz;
            }

        }

    }
}
