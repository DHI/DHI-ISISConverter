using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class JunctionClass : StructureClass
    {
        string Keyword = "JUNCTION";
        bool Energy = false;
        public bool IsRiverHead = true;

        public JunctionClass(string[] StArray, ref int i)
        {
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            bool OK = false;
            string stest = l.GetString(StArray[i], 1, ref OK);
            if (stest == "ENERGY") { Energy = true; };
            i++;
            ID = new LabelCollectionClass(StArray[i]);

        }

        public void SetConnections(List<RiverReachClass> RiverCollection)
        {
            string ConnectionRiverName = "";
            double ConnectionRiverChainage = 0;
            List<SectionBaseClass> XsecList;

            for (int i = 0; i < ID.Labels.Count; i++)
            {
                string label = ID.Labels[i];
                for (int ii = 0; ii < RiverCollection.Count; ii++)
                {
                    if (RiverCollection[ii].BranchType == RiverReachClass.BranchTypes.Regular)
                    {
                        XsecList = RiverCollection[ii].XsecCollection;
                        string RiverLabelup = XsecList[0].Label[0];
                        string RiverLabeldown = XsecList[XsecList.Count - 1].Label[0];

                        if (label == RiverLabelup)
                        {
                            if ((ConnectionRiverName == "") || ((ConnectionRiverName != "") & (!RiverCollection[ii].bStructureBranch)))
                            {
                                ConnectionRiverName = RiverCollection[ii].Name;
                                ConnectionRiverChainage = 0;


                            }

                        }
                        if (label == RiverLabeldown)
                        {
                            if ((ConnectionRiverName == "") || ((ConnectionRiverName != "") & (!RiverCollection[ii].bStructureBranch)))
                            {
                                ConnectionRiverName = RiverCollection[ii].Name;
                                ConnectionRiverChainage = RiverCollection[ii].EndChainage();


                            }

                        }
                    }

                }
            }
            for (int i = 0; i < ID.Labels.Count; i++)
            {
                string label = ID.Labels[i];
                for (int ii = 0; ii < RiverCollection.Count; ii++)
                {
                    if (RiverCollection[ii].BranchType == RiverReachClass.BranchTypes.Regular)
                    {
                        XsecList = RiverCollection[ii].XsecCollection;
                        string RiverLabelup = XsecList[0].Label[0];
                        string RiverLabeldown = XsecList[XsecList.Count - 1].Label[0];
                        if (label == RiverLabelup)
                        {
                            if (ConnectionRiverName != label)
                            {
                                RiverCollection[ii].UpStreamConnectionRiver = ConnectionRiverName;
                                RiverCollection[ii].UpStreamConnectionChainage = ConnectionRiverChainage;

                            }
                        }
                        if (label == RiverLabeldown)
                        {
                            if (ConnectionRiverName != label)
                            {
                                RiverCollection[ii].DownStreamConnectionRiver = ConnectionRiverName;
                                RiverCollection[ii].DownStreamConnectionChainage = ConnectionRiverChainage;

                            }
                        }
                    }

                }
            }
        }
    }
}

