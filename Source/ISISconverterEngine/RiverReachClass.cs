using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class RiverReachClass
    {
        public string Name;
        public double StartChainage = 0;
        public List<SectionBaseClass> XsecCollection;
        public List<GeoDigiPointClass> GeoPoints;
        public string DownStreamConnectionRiver = "";
        public double DownStreamConnectionChainage = 0;
        public string UpStreamConnectionRiver = "";
        public double UpStreamConnectionChainage = 0;
        public bool bStructureBranch = false;
        public enum BranchTypes { Regular, Link };
        public BranchTypes BranchType = BranchTypes.Regular;
        public RiverReachClass(string RiverName)
        {
            XsecCollection = new List<SectionBaseClass>();
            Name = RiverName;
            GeoPoints = new List<GeoDigiPointClass>();
        }

        public double EndChainage()
        {
            double endC = StartChainage;
            if (BranchType == BranchTypes.Regular)

                for (int i = 0; (i < XsecCollection.Count); i++)
                {
                    endC = endC + XsecCollection[i].deltaChainage;
                }
            else
                endC = 10; // All link channles are set to 10 m.

            return endC;
        }

        public void FixCrossSections(List<RiverReachClass> RiverReachCollection)
        {
            SectionBaseClass Xsec, XsecCopy;
            GeoDigiPointClass GeoPointConnection;
            if (bStructureBranch)
            {
                // upstream cross section
                if (UpStreamConnectionRiver == "")
                {
                    foreach (RiverReachClass riverreach in RiverReachCollection)
                    {
                        if (riverreach.UpStreamConnectionRiver == Name)
                        {
                            Xsec = riverreach.GetCrossSection(0);
                            if (riverreach.GeoPoints.Count > 0)
                            {
                                GeoPointConnection = riverreach.GeoPoints[0];
                                double alpha = Math.Min(10 / GeoPointConnection.Distance(GeoPoints[1]), 1);
                                double X = GeoPoints[1].X + alpha * (GeoPointConnection.X - GeoPoints[1].X);
                                double Y = GeoPoints[1].Y + alpha * (GeoPointConnection.Y - GeoPoints[1].Y);
                                GeoDigiPointClass lGeoPoint = new GeoDigiPointClass(X, Y, GeoPoints[0].Label);
                                GeoPoints.Insert(0, lGeoPoint);
                                GeoPoints.RemoveAt(1);
                            }
                            if (Xsec.GetType() == typeof(CrossSectionClass))
                            {
                                XsecCopy = new CrossSectionClass(Xsec as CrossSectionClass, 0, 0);
                            }
                            else
                            {
                                XsecCopy = new ConduitClass(Xsec as ConduitClass, 0, 0);
                            }
                            XsecCopy.Label = XsecCollection[0].Label;
                            XsecCopy.Comment = "Copy of " + Xsec.Label;
                            XsecCopy.deltaChainage = 10;
                            XsecCollection.Insert(0, XsecCopy);
                            XsecCollection.RemoveAt(1);
                        }
                    }



                }
                else
                {
                    foreach (RiverReachClass riverreach in RiverReachCollection)
                    {
                        if (UpStreamConnectionRiver == riverreach.Name)
                        {
                            Xsec = riverreach.GetCrossSection(UpStreamConnectionChainage);
                            if (riverreach.GeoPoints.Count > 0)
                            {
                                GeoPointConnection = riverreach.GetDigiPoint(UpStreamConnectionChainage);
                                double alpha = Math.Min(10 / GeoPointConnection.Distance(GeoPoints[1]), 1);
                                double X = GeoPoints[1].X + alpha * (GeoPointConnection.X - GeoPoints[1].X);
                                double Y = GeoPoints[1].Y + alpha * (GeoPointConnection.Y - GeoPoints[1].Y);
                                GeoDigiPointClass lGeoPoint = new GeoDigiPointClass(X, Y, GeoPoints[0].Label);
                                GeoPoints.Insert(0, lGeoPoint);
                                GeoPoints.RemoveAt(1);
                            }
                            if (Xsec.GetType() == typeof(CrossSectionClass))
                            {
                                XsecCopy = new CrossSectionClass(Xsec as CrossSectionClass, 0, 0);
                            }
                            else
                            {
                                XsecCopy = new ConduitClass(Xsec as ConduitClass, 0, 0);
                            }
                            XsecCopy.Label = XsecCollection[0].Label;
                            XsecCopy.Comment = "Copy of " + Xsec.Label;
                            XsecCopy.deltaChainage = 10;
                            XsecCollection.Insert(0, XsecCopy);
                            XsecCollection.RemoveAt(1);
                        }
                    }
                }
                // downstream cross section
                if (DownStreamConnectionRiver == "")
                {
                    foreach (RiverReachClass riverreach in RiverReachCollection)
                    {
                        if (riverreach.DownStreamConnectionRiver == Name)
                        {
                            Xsec = riverreach.GetCrossSection(riverreach.EndChainage());

                            if (Xsec.GetType() == typeof(CrossSectionClass))
                            {
                                XsecCopy = new CrossSectionClass(Xsec as CrossSectionClass, 0, 0);
                            }
                            else
                            {
                                XsecCopy = new ConduitClass(Xsec as ConduitClass, 0, 0);
                            }
                            XsecCopy.Label = XsecCollection[1].Label;
                            XsecCopy.Comment = "Copy of " + Xsec.Label;
                            XsecCopy.deltaChainage = 0;
                            XsecCollection.Insert(1, XsecCopy);
                            XsecCollection.RemoveAt(2);
                        }
                    }



                }
                else
                {
                    foreach (RiverReachClass riverreach in RiverReachCollection)
                    {
                        if (DownStreamConnectionRiver == riverreach.Name)
                        {
                            Xsec = riverreach.GetCrossSection(DownStreamConnectionChainage);

                            if (Xsec.GetType() == typeof(CrossSectionClass))
                            {
                                XsecCopy = new CrossSectionClass(Xsec as CrossSectionClass, 0, 0);
                            }
                            else
                            {
                                XsecCopy = new ConduitClass(Xsec as ConduitClass, 0, 0);
                            }
                            XsecCopy.Label = XsecCollection[1].Label;
                            XsecCopy.Comment = "Copy of " + Xsec.Label;
                            XsecCopy.deltaChainage = 0;
                            XsecCollection.Insert(1, XsecCopy);
                            XsecCollection.RemoveAt(2);
                        }
                    }
                }
            }
        }

        public SectionBaseClass GetCrossSection(double chain)
        {
            SectionBaseClass Xsec = XsecCollection[0];
            double Totchain = 0;
            bool found = false;
            for (int i = 0; i < XsecCollection.Count; i++)
            {
                Totchain = Totchain + XsecCollection[i].deltaChainage;
                if ((Math.Abs(Totchain - chain) < 0.0001) & (!found))
                {
                    Xsec = XsecCollection[i];
                    found = true;
                }
            }
            return Xsec;
        }

        public GeoDigiPointClass GetDigiPoint(double chain)
        {
            SectionBaseClass Xsec = XsecCollection[0];
            GeoDigiPointClass GeoPointReturn = GeoPoints[0];
            double Totchain = 0;
            bool found = false;
            for (int i = 0; i < XsecCollection.Count; i++)
            {
                Totchain = Totchain + XsecCollection[i].deltaChainage;
                if ((Math.Abs(Totchain - chain) < 0.0001) & (!found))
                {
                    Xsec = XsecCollection[i];
                    GeoPointReturn = GeoPoints[i];
                    found = true;
                }
            }
            return GeoPointReturn;
        }

        public void AddGeoinformation(HydraulicElementGeoCollectionClass GeoConnectionInfo)
        {
            for (int i = 0; (i < XsecCollection.Count); i++)
            {
                string xseclabel = XsecCollection[i].Label[0];

                for (int ii = 0; (ii < GeoConnectionInfo.ElementList.Count); ii++)
                {
                    HydraulicElementsGeoClass HydraulicElement = GeoConnectionInfo.ElementList[ii];
                    switch (HydraulicElement.Element)
                    {

                        case HydraulicElementsGeoClass.ElementTypes.CONDUIT_CIRCULAR:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.CONDUIT_FULLARCH:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.CONDUIT_RECTANGULAR:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.CONDUIT_SECTION:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.CONDUIT_SPRUNG:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.CONDUIT_SPRUNGARCH:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.INTERPOLATE:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.REPLICATE:
                            {
                                goto case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION;
                            }
                        case HydraulicElementsGeoClass.ElementTypes.RIVER_SECTION:
                            {
                                if (HydraulicElement.Label == xseclabel)
                                {
                                    GeoDigiPointClass lGeoPoint = new GeoDigiPointClass(HydraulicElement.GeoX, HydraulicElement.GeoY, HydraulicElement.Label);
                                    GeoPoints.Add(lGeoPoint);
                                }
                                break;
                            }
                    }

                }
            }
        }

        public void XsecGeoAutoMatch()
        {
            if (XsecCollection.Count == 0 || GeoPoints.Count == 0 || XsecCollection.Count != GeoPoints.Count)
            {
                return;
            }
            for (int i = 0; i < XsecCollection.Count-1; i++)
            {
                XsecCollection[i].deltaChainage = GetDistance(GeoPoints[i].X, GeoPoints[i].Y, GeoPoints[i+1].X, GeoPoints[i+1].Y);
            }
            XsecCollection[XsecCollection.Count - 1].deltaChainage = 0;
        }

        private double GetDistance(double x0, double y0, double x1, double y1)
        {
            return Math.Sqrt((x1-x0)*(x1-x0)+(y1-y0)*(y1-y0));
        }
    }
}
