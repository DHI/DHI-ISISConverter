using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class SpillClass
    {


        public MIKE11LinkChannelReachClass MakeLinkChannel(List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {
            String RName = "Link " + ID.Labels[0] + " to " + ID.Labels[1];
            MIKE11LinkChannelReachClass LLink = new MIKE11LinkChannelReachClass(RName, Surveydata);
            string labelup = ID.Labels[0];
            string labeldown = ID.Labels[1];

            LLink.bStructureBranch = false;
            LLink.StartChainage = 0;
            CrossSectionClass ldummyxsec = new CrossSectionClass();
            ldummyxsec.Label.Add(RName + "up");
            ldummyxsec.deltaChainage = 10;
            LLink.XsecCollection.Add(ldummyxsec);
            ldummyxsec = new CrossSectionClass();
            ldummyxsec.Label.Add(RName + "down");
            ldummyxsec.deltaChainage = 0;
            LLink.XsecCollection.Add(ldummyxsec);

            GeoDigiPointClass Gpoint = null;
            if (GeoPoint != null)
            {
                Gpoint = new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labelup);
                LLink.GeoPoints.Add(Gpoint);
                Gpoint = new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y-10, labelup);
                LLink.GeoPoints.Add(Gpoint);
            }

            foreach(StructureClass element in StructureList)
            {
                JunctionClass juncT = null;
                juncT = element as JunctionClass;
                for (int i = 0; i < element.ID.Labels.Count; i++)
                {
                    string structureLabel = element.ID.Labels[i];
                    RiverReachClass reach = StructureClass.FindReach(RiverCollection, element.RiverName);
                    if (reach == null)
                    {
                        continue;
                    }
                    if (labelup == structureLabel)
                    {
                        LLink.UpStreamConnectionRiver = element.RiverName;
                        if (juncT == null)
                        {
                            LLink.UpStreamConnectionChainage = reach.EndChainage() - reach.StartChainage;
                        }
                        else
                        {
                            if (juncT.IsRiverHead)
                            {
                                LLink.UpStreamConnectionChainage = reach.EndChainage() - reach.StartChainage;
                            }
                            else
                            {
                                LLink.UpStreamConnectionChainage = 0;
                            }
                        }

                    }
                    if (labeldown == structureLabel)
                    {
                        LLink.DownStreamConnectionRiver = element.RiverName;
                        if (juncT == null)
                        {
                            LLink.DownStreamConnectionChainage = 0;
                        }
                        else
                        {
                            if (juncT.IsRiverHead)
                            {
                                LLink.DownStreamConnectionChainage = reach.EndChainage() - reach.StartChainage;
                            }
                            else
                            {
                                LLink.DownStreamConnectionChainage = 0;
                            }
                        }
                    }
                }
            }

            for (int ii = 0; ii < RiverCollection.Count; ii++)
            {

                RiverReachClass River = RiverCollection[ii];
                if (LLink.UpStreamConnectionRiver == River.Name
                    || LLink.DownStreamConnectionRiver == River.Name)
                {
                    continue;
                }
                List<SectionBaseClass> XsecList = River.XsecCollection;
                double lchainage = 0;
                for (int iii = 0; iii < XsecList.Count; iii++)
                {
                    for (int iiii = 1; iiii < XsecList[iii].Label.Count; iiii++)
                    {
                        string RiverLabel = XsecList[iii].Label[iiii]; // get the spill labels

                        if (labelup == RiverLabel)
                        {
                            if (string.IsNullOrEmpty(LLink.UpStreamConnectionRiver))
                            {
                                LLink.UpStreamConnectionRiver = River.Name;
                                LLink.UpStreamConnectionChainage = lchainage;
                                if (GeoPoint != null)
                                {
                                    GeoDigiPointClass GeoPointConnection = River.GeoPoints[iii];
                                    double alpha = Math.Min(5 / GeoPointConnection.Distance(GeoPoint), 1);
                                    double X = GeoPoint.X + alpha * (GeoPointConnection.X - GeoPoint.X);
                                    double Y = GeoPoint.Y + alpha * (GeoPointConnection.Y - GeoPoint.Y);
                                    GeoDigiPointClass lGeoPoint = new GeoDigiPointClass(X, Y, labelup);
                                    LLink.GeoPoints.Insert(0, lGeoPoint);
                                    LLink.GeoPoints.RemoveAt(1);
                                }
                                else
                                {
                                    GeoDigiPointClass GeoPointConnection = River.GeoPoints[iii];
                                    GeoDigiPointClass lGeoPoint = new GeoDigiPointClass(GeoPointConnection.X, GeoPointConnection.Y, labelup);
                                    LLink.GeoPoints.Add(lGeoPoint);
                                }
                            }
                        }

                        if (labeldown == RiverLabel)
                        {
                            if (string.IsNullOrEmpty(LLink.DownStreamConnectionRiver))
                            {
                                LLink.DownStreamConnectionRiver = River.Name;
                                LLink.DownStreamConnectionChainage = lchainage;
                                if (GeoPoint != null)
                                {
                                    GeoDigiPointClass GeoPointConnection = River.GeoPoints[iii];
                                    double alpha = Math.Min(5 / GeoPointConnection.Distance(GeoPoint), 1);
                                    double X = GeoPoint.X + alpha * (GeoPointConnection.X - GeoPoint.X);
                                    double Y = GeoPoint.Y + alpha * (GeoPointConnection.Y - GeoPoint.Y);
                                    GeoDigiPointClass lGeoPoint = new GeoDigiPointClass(X, Y, labeldown);
                                    LLink.GeoPoints.Insert(1, lGeoPoint);
                                    LLink.GeoPoints.RemoveAt(2);
                                }
                                else
                                {
                                    GeoDigiPointClass GeoPointConnection = River.GeoPoints[iii];
                                    GeoDigiPointClass lGeoPoint = new GeoDigiPointClass(GeoPointConnection.X, GeoPointConnection.Y, labeldown);
                                    LLink.GeoPoints.Add(lGeoPoint);
                                }
                            }
                        }

                    }

                    lchainage = lchainage + XsecList[iii].deltaChainage;
                }




            }

            return LLink;

        }

        public class SurveyDataClass
        {
            public double x = 0, z = 0;
        }

        public String Keyword = "UNDEFINED";
        public String Keyword2;
        public string Comment = "";
        public LabelCollectionClass ID;
        public double Coefficient = 0;
        public double ModularLimit = 0.9;
        public int NumberOfPoints;
        public SurveyDataClass[] Surveydata;

        public GeoDigiPointClass GeoPoint;
        public SpillClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "SPILL";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            Coefficient = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            ModularLimit = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            i++;
            NumberOfPoints = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            Surveydata = new SurveyDataClass[NumberOfPoints];
            for (int ii = 0; ii < NumberOfPoints; ii++)
            {
                i++;
                Surveydata[ii] = new SurveyDataClass();
                Surveydata[ii].x = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                Surveydata[ii].z = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            }

        }

        public bool SetGeoPoint(HydraulicElementsGeoClass.ElementTypes elementtype, List<HydraulicElementsGeoClass> ElementList)
        {
            bool success = false;
            if (ElementList.Count > 0)
                foreach (HydraulicElementsGeoClass element in ElementList)
                {
                    if (element.Element == elementtype)
                    {
                        if (element.Label == ID.Labels[0])
                        {
                            GeoPoint = new GeoDigiPointClass(element.GeoX, element.GeoY, element.Label);

                            success = true;
                        }
                    }

                }
            return success;
        }


    }
}
