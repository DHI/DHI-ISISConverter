using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class StructureClass
    {
        public String Keyword = "UNDEFINED";
        public String Keyword2;
        public string Comment = "";
        public LabelCollectionClass ID;
        public string RiverName = "";
        public double Chainage = 0;
        public GeoDigiPointClass GeoPoint;

        public bool SetGeoPoint(HydraulicElementsGeoClass.ElementTypes elementtype, List<HydraulicElementsGeoClass> ElementList)
        {
            bool success = false;
            if (ElementList.Count > 0)
                foreach (HydraulicElementsGeoClass element in ElementList)
                {
                    if (element.Element == elementtype || element.Element == HydraulicElementsGeoClass.ElementTypes.NOTDEFINED)
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


        public RiverReachClass CreateBranch(List<RiverReachClass> RiverCollection)
        {
            string labelup = ID.Labels[0];
            string labeldown = string.Empty;
            if (ID.Labels.Count > 1)
            {
                labeldown = ID.Labels[1];
            }
            else
            {
                labeldown = "";
            }
            RiverReachClass StructureReach;
            SectionBaseClass lxsec;
            RiverName = "River " + (RiverCollection.Count + 1).ToString();
            Chainage = 5;
            StructureReach = new RiverReachClass(RiverName);
            StructureReach.bStructureBranch = true;
            StructureReach.StartChainage = 0;
            CrossSectionClass ldummyxsec = new CrossSectionClass();
            ldummyxsec.Label.Add(labelup);
            ldummyxsec.deltaChainage = 10;
            StructureReach.XsecCollection.Add(ldummyxsec);
            ldummyxsec = new CrossSectionClass();
            ldummyxsec.Label.Add(labeldown);
            ldummyxsec.deltaChainage = 0;
            StructureReach.XsecCollection.Add(ldummyxsec);
            GeoDigiPointClass Gpoint = null;
            if (GeoPoint != null)
            {
                Gpoint = new GeoDigiPointClass(GeoPoint.X - 10, GeoPoint.Y, labelup);
                StructureReach.GeoPoints.Add(Gpoint);
                Gpoint = new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labelup);
                StructureReach.GeoPoints.Add(Gpoint);
            }


            for (int ii = 0; ii < RiverCollection.Count; ii++)
            {

                RiverReachClass River = RiverCollection[ii];
                List<SectionBaseClass> XsecList = River.XsecCollection;
                double lchainage = 0;
                for (int iii = 0; iii < XsecList.Count; iii++)
                {
                    string RiverLabel = XsecList[iii].Label[0];

                    if (labeldown == RiverLabel)
                    {
                        StructureReach.DownStreamConnectionRiver = River.Name;
                        StructureReach.DownStreamConnectionChainage = lchainage;

                        if (GeoPoint != null) Gpoint = River.GeoPoints[iii];

                        if (XsecList[iii].GetType() == typeof(CrossSectionClass))
                        {

                            lxsec = new CrossSectionClass(XsecList[iii] as CrossSectionClass, 0, 0);

                        }
                        else
                        {
                            lxsec = new ConduitClass(XsecList[iii] as ConduitClass, 0, 0);

                        }
                        lxsec.Label.Add(labeldown);
                        lxsec.deltaChainage = 0;
                        StructureReach.XsecCollection.Insert(1, lxsec);
                        StructureReach.XsecCollection.RemoveAt(2);
                        if (GeoPoint != null)
                        {
                            StructureReach.GeoPoints.Insert(1, Gpoint);
                            StructureReach.GeoPoints.RemoveAt(2);
                        }
                    }
                    if (labelup == RiverLabel)
                    {
                        StructureReach.UpStreamConnectionRiver = River.Name;
                        StructureReach.UpStreamConnectionChainage = lchainage;
                        if (GeoPoint != null) Gpoint = River.GeoPoints[iii];

                        if (XsecList[iii].GetType() == typeof(CrossSectionClass))
                        {

                            lxsec = new CrossSectionClass(XsecList[iii] as CrossSectionClass, 0, 0);

                        }
                        else
                        {
                            lxsec = new ConduitClass(XsecList[iii] as ConduitClass, 0, 0);

                        }
                        lxsec.Label.Add(labelup);
                        lxsec.deltaChainage = 10;
                        StructureReach.XsecCollection.Insert(0, lxsec);
                        StructureReach.XsecCollection.RemoveAt(1);
                        if (GeoPoint != null)
                        {
                            StructureReach.GeoPoints.Insert(0, Gpoint);
                            StructureReach.GeoPoints.RemoveAt(1);
                        }
                    }



                }



            }
            return StructureReach;
        }

        public void CreateSingleBranch(ref List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {
            List<string> strTemp = new List<string>();
            JunctionClass juncT = null;
            string labelup = ID.Labels[0];
            string labeldown = string.Empty;

            RiverReachClass StructureReach;
            SectionBaseClass lxsec;
            RiverName = "River " + (RiverCollection.Count + 1).ToString();
            Chainage = 5;
            StructureReach = new RiverReachClass(RiverName);
            StructureReach.bStructureBranch = true;
            StructureReach.StartChainage = 0;
            CrossSectionClass ldummyxsec = new CrossSectionClass();
            ldummyxsec.Label.Add(labelup);
            ldummyxsec.deltaChainage = 10;
            StructureReach.XsecCollection.Add(ldummyxsec);
            ldummyxsec = new CrossSectionClass();
            ldummyxsec.Label.Add(labeldown);
            ldummyxsec.deltaChainage = 0;
            StructureReach.XsecCollection.Add(ldummyxsec);
            GeoDigiPointClass Gpoint = null;
            if (GeoPoint != null)
            {
                Gpoint = new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labelup);
                StructureReach.GeoPoints.Add(Gpoint);
                Gpoint = new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labelup);
                StructureReach.GeoPoints.Add(Gpoint);
            }

            if (StructureList.Count > 0)
            {
                foreach (StructureClass element in StructureList)
                {
                    string StructureLabel = "";
                    StructureLabel = element.ID.Labels[0];

                    if (element.GeoPoint == null)
                    {
                        continue;
                    }

                    GeoDigiPointClass GpointS = null;
                    GpointS = element.GeoPoint;
                    if (labelup == StructureLabel)
                    {
                        if (GeoPoint != null)
                        {
                            if (strTemp.Contains(StructureLabel))
                            {
                                continue;
                            }
                            strTemp.Add(StructureLabel);
                            StructureReach.GeoPoints.Insert(0, GpointS);
                            StructureReach.GeoPoints.RemoveAt(1);

                            CrossSectionClass ldummyxsecP = new CrossSectionClass();
                            ldummyxsecP.Label.Add(StructureLabel);
                            ldummyxsecP.deltaChainage = 10;
                            StructureReach.XsecCollection.Insert(0, ldummyxsecP);
                            StructureReach.XsecCollection.RemoveAt(1);

                            juncT = element as JunctionClass;
                        }
                    }
                }
            }

            for (int ii = 0; ii < RiverCollection.Count; ii++)
            {

                RiverReachClass River = RiverCollection[ii];
                List<SectionBaseClass> XsecList = River.XsecCollection;
                double lchainage = 0;
                for (int iii = 0; iii < XsecList.Count; iii++)
                {
                    string RiverLabel = XsecList[iii].Label[0];
                    if (labelup == RiverLabel)
                    {
                        StructureReach.UpStreamConnectionRiver = River.Name;
                        StructureReach.UpStreamConnectionChainage = lchainage;
                        if (GeoPoint != null) Gpoint = River.GeoPoints[iii];

                        if (XsecList[iii].GetType() == typeof(CrossSectionClass))
                        {

                            lxsec = new CrossSectionClass(XsecList[iii] as CrossSectionClass, 0, 0);

                        }
                        else
                        {
                            lxsec = new ConduitClass(XsecList[iii] as ConduitClass, 0, 0);

                        }
                        lxsec.Label.Add(labelup);
                        lxsec.deltaChainage = 10;
                        StructureReach.XsecCollection.Insert(0, lxsec);
                        StructureReach.XsecCollection.RemoveAt(1);
                        if (GeoPoint != null)
                        {
                            if (strTemp.Contains(RiverLabel))
                            {
                                continue;
                            }
                            strTemp.Add(RiverLabel);
                            StructureReach.GeoPoints.Insert(0, Gpoint);
                            StructureReach.GeoPoints.RemoveAt(1);
                        }
                    }

                }
            }

            juncT = this as JunctionClass;
            if (juncT == null)
            {
                RiverCollection.Add(StructureReach);
            }

        }

        public void CreateJuncBranch(ref List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {

            List<string> strTemp = new List<string>();
            JunctionClass juncT = null;
            juncT = this as JunctionClass;
            if (juncT == null)
            {
                return;
            }
            string labelup = ID.Labels[0];


            GeoDigiPointClass Gpoint = null;


            for (int i = 1; i < ID.Labels.Count; i++)
            {
                string labeldown = ID.Labels[i];

                foreach (StructureClass element in StructureList)
                {
                    JunctionClass juncTemp = null;
                    juncTemp = element as JunctionClass;
                    if (juncTemp != null)
                    {
                        continue;
                    }
                    string StructureLabel = "";
                    StructureLabel = element.ID.Labels[0];

                    if (element.GeoPoint == null)
                    {
                        continue;
                    }

                    GeoDigiPointClass GpointS = null;
                    GpointS = element.GeoPoint;
                    if (labelup == StructureLabel)
                    {
                        if (strTemp.Contains(StructureLabel))
                        {
                            continue;
                        }
                        strTemp.Add(StructureLabel);

                        if (GeoPoint != null)
                        {
                            //new river
                            RiverReachClass StructureReach;
                            SectionBaseClass lxsec;
                            RiverName = "River " + (RiverCollection.Count + 1).ToString();
                            Chainage = 5;
                            StructureReach = new RiverReachClass(RiverName);
                            StructureReach.bStructureBranch = true;
                            StructureReach.StartChainage = 0;

                            StructureReach.GeoPoints.Add(new GeoDigiPointClass(GpointS.X, GpointS.Y, labelup));
                            CrossSectionClass ldummyxsec = new CrossSectionClass();
                            ldummyxsec.Label.Add(labelup);
                            ldummyxsec.deltaChainage = 10;
                            StructureReach.XsecCollection.Add(ldummyxsec);

                            StructureReach.GeoPoints.Add(GpointS);
                            CrossSectionClass ldummyxsecP = new CrossSectionClass();
                            ldummyxsecP.Label.Add(StructureLabel);
                            ldummyxsecP.deltaChainage = 0;
                            StructureReach.XsecCollection.Add(ldummyxsecP);

                            RiverCollection.Add(StructureReach);

                            //end newriver
                            element.RiverName = RiverName;
                        }
                    }
                    if (labeldown == StructureLabel)
                    {
                        if (strTemp.Contains(StructureLabel))
                        {
                            continue;
                        }
                        strTemp.Add(StructureLabel);

                        if (GeoPoint != null)
                        {
                            //new river
                            RiverReachClass StructureReach;
                            SectionBaseClass lxsec;
                            RiverName = "River " + (RiverCollection.Count + 1).ToString();
                            Chainage = 5;
                            StructureReach = new RiverReachClass(RiverName);
                            StructureReach.bStructureBranch = true;
                            StructureReach.StartChainage = 0;

                            StructureReach.GeoPoints.Add(new GeoDigiPointClass(GpointS.X, GpointS.Y, labelup));
                            CrossSectionClass ldummyxsec = new CrossSectionClass();
                            ldummyxsec.Label.Add(labelup);
                            ldummyxsec.deltaChainage = 10;
                            StructureReach.XsecCollection.Add(ldummyxsec);

                            StructureReach.GeoPoints.Add(GpointS);
                            CrossSectionClass ldummyxsecP = new CrossSectionClass();
                            ldummyxsecP.Label.Add(StructureLabel);
                            ldummyxsecP.deltaChainage = 0;
                            StructureReach.XsecCollection.Add(ldummyxsecP);

                            RiverCollection.Add(StructureReach);

                            //end new river
                            element.RiverName = RiverName;
                        }
                    }
                }

                for (int ii = 0; ii < RiverCollection.Count; ii++)
                {

                    RiverReachClass River = RiverCollection[ii];
                    List<SectionBaseClass> XsecList = River.XsecCollection;
                    double lchainage = 0;
                    for (int iii = 0; iii < XsecList.Count; iii++)
                    {
                        string RiverLabel = XsecList[iii].Label[0];

                        if (labeldown == RiverLabel)
                        {
                            if (strTemp.Contains(RiverLabel))
                            {
                                continue;
                            }
                            strTemp.Add(RiverLabel);

                            //new river
                            RiverReachClass StructureReach;
                            SectionBaseClass lxsec;
                            RiverName = "River " + (RiverCollection.Count + 1).ToString();
                            Chainage = 5;
                            StructureReach = new RiverReachClass(RiverName);
                            StructureReach.bStructureBranch = true;
                            StructureReach.StartChainage = 0;

                            StructureReach.DownStreamConnectionRiver = River.Name;
                            StructureReach.DownStreamConnectionChainage = lchainage;

                            if (GeoPoint != null) Gpoint = River.GeoPoints[iii];

                            StructureReach.GeoPoints.Add(new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labelup));
                            CrossSectionClass ldummyxsec = new CrossSectionClass();
                            ldummyxsec.Label.Add(labelup);
                            ldummyxsec.deltaChainage = 10;
                            StructureReach.XsecCollection.Add(ldummyxsec);

                            if (XsecList[iii].GetType() == typeof(CrossSectionClass))
                            {

                                lxsec = new CrossSectionClass(XsecList[iii] as CrossSectionClass, 0, 0);

                            }
                            else
                            {
                                lxsec = new ConduitClass(XsecList[iii] as ConduitClass, 0, 0);

                            }
                            lxsec.Label.Add(labeldown);
                            lxsec.deltaChainage = 0;
                            StructureReach.XsecCollection.Add(lxsec);
                            StructureReach.GeoPoints.Add(new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labeldown));

                            RiverCollection.Add(StructureReach);

                            //end new river                 
                        }
                        if (labelup == RiverLabel)
                        {
                            if (strTemp.Contains(RiverLabel))
                            {
                                continue;
                            }
                            strTemp.Add(RiverLabel);
                            //new river
                            RiverReachClass StructureReach;
                            SectionBaseClass lxsec;
                            RiverName = "River " + (RiverCollection.Count + 1).ToString();
                            Chainage = 5;
                            StructureReach = new RiverReachClass(RiverName);
                            StructureReach.bStructureBranch = true;
                            StructureReach.StartChainage = 0;

                            StructureReach.UpStreamConnectionRiver = River.Name;
                            StructureReach.UpStreamConnectionChainage = lchainage;

                            if (GeoPoint != null) Gpoint = River.GeoPoints[iii];

                            StructureReach.GeoPoints.Add(new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labelup));
                            CrossSectionClass ldummyxsec = new CrossSectionClass();
                            ldummyxsec.Label.Add(labelup);
                            ldummyxsec.deltaChainage = 10;
                            StructureReach.XsecCollection.Add(ldummyxsec);

                            if (XsecList[iii].GetType() == typeof(CrossSectionClass))
                            {

                                lxsec = new CrossSectionClass(XsecList[iii] as CrossSectionClass, 0, 0);

                            }
                            else
                            {
                                lxsec = new ConduitClass(XsecList[iii] as ConduitClass, 0, 0);

                            }
                            lxsec.Label.Add(labelup);
                            lxsec.deltaChainage = 10;
                            StructureReach.XsecCollection.Add(lxsec);
                            StructureReach.GeoPoints.Add(new GeoDigiPointClass(GeoPoint.X, GeoPoint.Y, labelup));

                            RiverCollection.Add(StructureReach);

                            //end new river     
                        }

                    }
                }
            }

        }

        public static void CreateAllBranch(ref List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {
            string StructureLabel = "";
            string RiverName = "";
            double Chainage = 0;
            foreach (StructureClass element in StructureList)
            {
                JunctionClass juncT = null;
                juncT = element as JunctionClass;
                if (juncT != null)
                {
                    continue;
                }
                StructureLabel = element.ID.Labels[0];

                if (element.GeoPoint == null)
                {
                    continue;
                }

                GeoDigiPointClass GpointS = null;
                GpointS = element.GeoPoint;
                //new river
                RiverReachClass StructureReach;
                RiverName = "River " + (RiverCollection.Count + 1).ToString();
                Chainage = 5;
                StructureReach = new RiverReachClass(RiverName);
                StructureReach.bStructureBranch = true;
                StructureReach.StartChainage = 0;

                StructureReach.GeoPoints.Add(new GeoDigiPointClass(GpointS.X, GpointS.Y, StructureLabel));
                CrossSectionClass ldummyxsec = new CrossSectionClass();
                ldummyxsec.Label.Add(StructureLabel);
                ldummyxsec.deltaChainage = 10;
                StructureReach.XsecCollection.Add(ldummyxsec);

                StructureReach.GeoPoints.Add(new GeoDigiPointClass(GpointS.X, GpointS.Y - 10, StructureLabel));
                CrossSectionClass ldummyxsecP = new CrossSectionClass();
                ldummyxsecP.Label.Add(StructureLabel);
                ldummyxsecP.deltaChainage = 0;
                StructureReach.XsecCollection.Add(ldummyxsecP);

                RiverCollection.Add(StructureReach);
                //end newriver
                element.RiverName = RiverName;
            }
        }

        public static void SetJunctionRiver(List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {
            foreach (StructureClass element in StructureList)
            {
                JunctionClass juncT = null;
                juncT = element as JunctionClass;
                if (juncT == null)
                {
                    continue;
                }
                List<SectionBaseClass> XsecList;

                for (int i = 0; i < element.ID.Labels.Count; i++)
                {
                    string label = element.ID.Labels[i];
                    for (int ii = 0; ii < RiverCollection.Count; ii++)
                    {
                        RiverReachClass river = RiverCollection[ii];
                        XsecList = RiverCollection[ii].XsecCollection;
                        for (int iii = 0; iii < XsecList.Count; iii++)
                        {
                            for (int iiii = 0; iiii < XsecList[iii].Label.Count; iiii++)
                            {
                                if (label == XsecList[iii].Label[iiii])
                                {
                                    element.RiverName = river.Name;
                                    if (iii == 0)
                                    {
                                        juncT.IsRiverHead = false;
                                    }
                                    else
                                    {
                                        juncT.IsRiverHead = true;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SetJuncriverStructure(List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {
            foreach (StructureClass element in StructureList)
            {
                JunctionClass juncT = null;
                juncT = element as JunctionClass;
                if (juncT == null || !string.IsNullOrEmpty(element.RiverName))
                {
                    continue;
                }
                foreach (StructureClass elementT in StructureList)
                {
                    juncT = element as JunctionClass;
                    if (juncT != null)
                    {
                        continue;
                    }
                    for (int i = 0; i < element.ID.Labels.Count; i++)
                    {
                        string label = element.ID.Labels[i];
                        for (int j = 0; j < elementT.ID.Labels.Count; j++)
                        {
                            if (label == elementT.ID.Labels[j])
                            {
                                element.RiverName = elementT.RiverName;
                                break;
                            }
                        }
                    }
                }

            }
        }

        public void SetAllConnections(ref List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {
            JunctionClass juncT = null;
            juncT = this as JunctionClass;
            if (juncT != null)
            {
                return;
            }

            RiverReachClass reach = FindReach(RiverCollection, this.RiverName);
            if (reach == null)
            {
                return;
            }

            double lchainage = 0;

            string labelup = ID.Labels[0];
            string labeldown = "";
            if (ID.Labels.Count > 1)
            {
                labeldown = ID.Labels[1];
            }

            foreach (StructureClass element in StructureList)
            {
                if (element == this)
                {
                    continue;
                }

                string StructureLabel = "";

                juncT = element as JunctionClass;
                if (juncT != null)
                {
                    for (int j = 0; j < element.ID.Labels.Count; j++)
                    {
                        StructureLabel = element.ID.Labels[j];

                        RiverReachClass reachT = null;
                        reachT = FindReach(RiverCollection, element.RiverName);
                        if (reachT == null)
                        {
                            continue;
                        }
                        if (labelup == StructureLabel )
                        {
                            reach.UpStreamConnectionRiver = element.RiverName;
                            if (juncT.IsRiverHead)
                            {
                                reach.UpStreamConnectionChainage = reachT.EndChainage();
                            }
                            else
                            {
                                reach.UpStreamConnectionChainage = 0;
                            }
                        }
                        if (labeldown == StructureLabel )
                        {
                            reach.DownStreamConnectionRiver = element.RiverName;
                            if (juncT.IsRiverHead)
                            {
                                reach.DownStreamConnectionChainage = reachT.EndChainage();
                            }
                            else
                            {
                                reach.DownStreamConnectionChainage = 0;
                            }
                        }
                    }
                }
                else
                {
                    StructureLabel = element.ID.Labels[0];

                    RiverReachClass reachT = null;
                    reachT = FindReach(RiverCollection, element.RiverName);
                    if (reachT == null)
                    {
                        continue;
                    }
                    if (labelup == StructureLabel)
                    {
                        reach.UpStreamConnectionRiver = element.RiverName;
                        reach.UpStreamConnectionChainage = reachT.EndChainage();
                    }
                    if (labeldown == StructureLabel)
                    {
                        reach.DownStreamConnectionRiver = element.RiverName;
                        reach.DownStreamConnectionChainage = 0;
                    }
                }
            }

            for (int ii = 0; ii < RiverCollection.Count; ii++)
            {
                RiverReachClass River = RiverCollection[ii];
                if (River == reach)
                {
                    continue;
                }
                if (reach.DownStreamConnectionRiver == River.Name
                    || reach.UpStreamConnectionRiver == River.Name)
                {
                    continue;
                }
                List<SectionBaseClass> XsecList = River.XsecCollection;

                for (int iii = 0; iii < XsecList.Count; iii++)
                {
                    for (int iiii = 0; iiii < XsecList[iii].Label.Count; iiii++)
                    {
                        string RiverLabel = XsecList[iii].Label[iiii];

                        if (labeldown == RiverLabel)
                        {
                            if (string.IsNullOrEmpty(reach.DownStreamConnectionRiver))
                            {
                                reach.DownStreamConnectionRiver = River.Name;
                                if (iii == 0)
                                {
                                    reach.DownStreamConnectionChainage = 0;
                                }
                                else
                                {
                                    lchainage = River.EndChainage() - River.StartChainage;
                                    reach.DownStreamConnectionChainage = lchainage;
                                }
                            }
                        }
                        if (labelup == RiverLabel)
                        {
                            if (string.IsNullOrEmpty(reach.UpStreamConnectionRiver))
                            {
                                reach.UpStreamConnectionRiver = River.Name;
                                if (iii == 0)
                                {
                                    reach.UpStreamConnectionChainage = 0;
                                }
                                else
                                {
                                    lchainage = River.EndChainage() - River.StartChainage;
                                    reach.UpStreamConnectionChainage = lchainage;
                                }
                            }
                        }
                    }

                }
            }

        }

        public void SetJunConnections(ref List<RiverReachClass> RiverCollection, List<StructureClass> StructureList)
        {
            JunctionClass juncT = null;
            juncT = this as JunctionClass;
            if (juncT == null)
            {
                return;
            }

            RiverReachClass reachT = FindReach(RiverCollection, this.RiverName);
            if (reachT == null)
            {
                return;
            }

            for (int i = 0; i < ID.Labels.Count; i++)
            {
                string label = ID.Labels[i];

                foreach (StructureClass element in StructureList)
                {
                    if (element == this)
                    {
                        continue;
                    }

                    RiverReachClass reach = null;
                    reach = FindReach(RiverCollection, element.RiverName);
                    if (reach == null)
                    {
                        continue;
                    }

                    if (reach.UpStreamConnectionRiver == this.RiverName
                        || reach.DownStreamConnectionRiver == this.RiverName)
                    {
                        continue;
                    }

                    JunctionClass juncTT = null;
                    juncTT = element as JunctionClass;
                    if (juncTT != null)
                    {
                        for (int j = 0; j < element.ID.Labels.Count; j++)
                        {
                            string StructureLabel = "";
                            StructureLabel = element.ID.Labels[j];
                            if (label == StructureLabel)
                            {
                                if (juncTT.IsRiverHead)
                                {
                                    if (string.IsNullOrEmpty(reach.DownStreamConnectionRiver))
                                    {
                                        reach.DownStreamConnectionRiver = this.RiverName;
                                        if (juncT.IsRiverHead)
                                        {
                                            reach.DownStreamConnectionChainage = reachT.EndChainage() - reachT.StartChainage;
                                        }
                                        else
                                        {
                                            reach.DownStreamConnectionChainage = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(reach.UpStreamConnectionRiver))
                                    {
                                        reach.UpStreamConnectionRiver = this.RiverName;
                                        if (juncT.IsRiverHead)
                                        {
                                            reach.UpStreamConnectionChainage = reachT.EndChainage() - reachT.StartChainage;
                                        }
                                        else
                                        {
                                            reach.UpStreamConnectionChainage = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string labelup = element.ID.Labels[0];
                        string labeldown = "";
                        if (element.ID.Labels.Count > 1)
                        {
                            labeldown = element.ID.Labels[1];
                        }
                        if (label == labelup)
                        {
                            if (string.IsNullOrEmpty(reach.UpStreamConnectionRiver))
                            {
                                reach.UpStreamConnectionRiver = element.RiverName;
                                if (juncT.IsRiverHead)
                                {
                                    reach.UpStreamConnectionChainage = reachT.EndChainage();
                                }
                                else
                                {
                                    reach.UpStreamConnectionChainage = 0;
                                }
                            }
                        }
                        if (label == labeldown)
                        {
                            if (string.IsNullOrEmpty(reach.DownStreamConnectionRiver))
                            {
                                reach.DownStreamConnectionRiver = this.RiverName;
                                if (juncT.IsRiverHead)
                                {
                                    reach.DownStreamConnectionChainage = reachT.EndChainage();
                                }
                                else
                                {
                                    reach.DownStreamConnectionChainage = 0;
                                }
                            }
                        }

                    }
                }

                for (int ii = 0; ii < RiverCollection.Count; ii++)
                {
                    RiverReachClass River = RiverCollection[ii];

                    if (River == reachT)
                    {
                        continue;
                    }

                    List<SectionBaseClass> XsecList = River.XsecCollection;

                    RiverReachClass reach = null;
                    reach = FindReach(RiverCollection, River.Name);

                    if (reach.UpStreamConnectionRiver == this.RiverName
                        || reach.DownStreamConnectionRiver == this.RiverName)
                    {
                        continue;
                    }

                    for (int iii = 0; iii < XsecList.Count; iii++)
                    {
                        string RiverLabel = XsecList[iii].Label[0];

                        if (label == RiverLabel)
                        {
                            if (iii == 0)
                            {
                                if (string.IsNullOrEmpty(reach.UpStreamConnectionRiver))
                                {
                                    reach.UpStreamConnectionRiver = this.RiverName;
                                    if (juncT.IsRiverHead)
                                    {
                                        reach.UpStreamConnectionChainage = reachT.EndChainage() - reachT.StartChainage;
                                    }
                                    else
                                    {
                                        reach.UpStreamConnectionChainage = 0;
                                    }
                                }

                            }
                            else
                            {
                                if (string.IsNullOrEmpty(reach.DownStreamConnectionRiver))
                                {
                                    reach.DownStreamConnectionRiver = this.RiverName;
                                    if (juncT.IsRiverHead)
                                    {
                                        reach.DownStreamConnectionChainage = reachT.EndChainage() - reachT.StartChainage;
                                    }
                                    else
                                    {
                                        reach.DownStreamConnectionChainage = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static RiverReachClass FindReach(List<RiverReachClass> RiverCollection, string name)
        {
            RiverReachClass reach = null;

            for (int i = 0; i < RiverCollection.Count; i++)
            {
                if (RiverCollection[i].Name == name)
                {
                    reach = RiverCollection[i];
                    break;
                }
            }

            return reach;
        }

        public virtual MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {

            MIKE11StructureClass M11struc = new MIKE11StructureClass(lstructure);
            M11struc.Chainage = Chainage;
            M11struc.RiverName = RiverName;
            M11struc.ID = Keyword + ID.Labels[0] + Comment;

            return M11struc;
        }
    }

}
