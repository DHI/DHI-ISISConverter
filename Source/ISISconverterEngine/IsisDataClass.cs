using System;
using System.Collections.Generic;
using System.Linq;
using DHI.MHydro.Data;
using DHI.Mike1D.Generic;
using ThinkGeo.Core;
using DHI.SetupEditorEx.Data;

namespace ISISConverterEngine
{
    public class IsisDataClass
    {
        private RootPfs rootPfs;

        public bool GeoLocationsExists = false;
        private string[] inputFilestringarray;
        public List<StructureClass> StructureCollection;

        public List<AbstractionClass> AbstractionCollection;
        public List<BernoulliLossClass> BernoulliLossCollection;
        public List<BlockageClass> BlockageCollection;
        public List<QHBoundaryClass> QHBoundaryCollection;
        public List<HtBoundaryClass> HtBoundaryCollection;
        public List<QtBoundaryClass> QtBoundaryCollection;
        public List<TidalBoundaryClass> TidalBoundaryCollection;
        public List<RainEvapInfiltrationClass> RainEvapInfiltrationCollection;
        public List<NormalCriticalBoundaryClass> NormalCriticalBoundaryCollection;
        public List<BridgeClass> BridgeCollection;
        // comment
        public List<ConduitClass> ConduitCollection;
        public List<CulvertClass> CulvertCollection;
        public List<FloodPlainClass> FloodPlainCollection;
        // gauge
        public List<HeadLossClass> HeadLossCollection;
        // Hydrological boundaries
        // interpolate see RIVER 
        public List<JunctionClass> JunctionCollection;
        public List<LateralClass> LateralCollection;
        public List<ManholeClass> ManholeCollection;
        public List<OrificeClass> OrificeCollection;
        public List<PondClass> PondCollection;
        public List<PumpClass> PumpCollection;
        public List<RatingCurveClass> RatingCurveCollection;
        // Replicate see RIVER
        public List<ReservoirClass> ReservoirCollection;
        // River start see River reach collection
        // CES-SECTION
        // MUSK-VPMC
        // MUSK-XSEC
        // MUSK-RSEC
        public MuskingumCollectionClass MuskingumCollection;
        // River end
        // Rules
        public List<SluiceClass> SluiceCollection;
        public List<SpillClass> SpillCollection;
        public List<CrumpClass> CrumpCollection;
        public List<GatedWeirClass> GatedWeirCollection;
        public List<NotionalWeirClass> NotionalWeirCollection;
        public List<FlatVWeirClass> FlatVWeirCollection;
        public List<QHControlClass> QHControlCollection;
        public List<RoundNosedBroadCrestedWeirClass> RoundNoseBroadCrestedWeirCollection;
        public List<LabyrinthWeirClass> LabyrinthWeirCollection;
        public List<SharpCrestedWeirClass> SharpCrestedWeirCollection;
        public List<SyphonClass> SyphonCollection;
        public List<WeirClass> WeirCollection;
        // WIND

        public HydraulicElementGeoCollectionClass HydraulicElementsCollection;

        public List<RiverReachClass> RiverReachCollection;

        public List<MIKE11StructureClass> MIKE11StructureCollection;
        public List<MIKE11BoundaryClass> MIKE11BoundaryCollection;

        public List<int> errLineNums = new List<int>();

        Dictionary<string, Guid> nameGuidDict = new Dictionary<string, Guid>();
        //simulation period and timestep
        public static DateTime datetimeStart = new DateTime();
        public static double duration = 0;
        public double timestep = 0;

        public bool IsRiver = false;

        public IsisDataClass()
        {
            rootPfs = new RootPfs();
            StructureCollection = new List<StructureClass>();
            AbstractionCollection = new List<AbstractionClass>();
            BernoulliLossCollection = new List<BernoulliLossClass>();
            BlockageCollection = new List<BlockageClass>();
            QHBoundaryCollection = new List<QHBoundaryClass>();
            HtBoundaryCollection = new List<HtBoundaryClass>();
            QtBoundaryCollection = new List<QtBoundaryClass>();
            TidalBoundaryCollection = new List<TidalBoundaryClass>();
            RainEvapInfiltrationCollection = new List<RainEvapInfiltrationClass>();
            NormalCriticalBoundaryCollection = new List<NormalCriticalBoundaryClass>();
            BridgeCollection = new List<BridgeClass>();
            //comment
            ConduitCollection = new List<ConduitClass>();
            CulvertCollection = new List<CulvertClass>();
            FloodPlainCollection = new List<FloodPlainClass>();
            // gauge
            HeadLossCollection = new List<HeadLossClass>();
            // Hydrological boundaries
            // interpolate see RIVER 
            JunctionCollection = new List<JunctionClass>();
            LateralCollection = new List<LateralClass>();
            ManholeCollection = new List<ManholeClass>();
            OrificeCollection = new List<OrificeClass>();
            PondCollection = new List<PondClass>();
            PumpCollection = new List<PumpClass>();
            RatingCurveCollection = new List<RatingCurveClass>();
            // Replicate see RIVER
            ReservoirCollection = new List<ReservoirClass>();
            // River start see River reach collection
            // CES-SECTION
            // MUSK-VPMC
            // MUSK-XSEC
            // MUSK-RSEC
            MuskingumCollection = new MuskingumCollectionClass();
            // River end
            // Rules
            SluiceCollection = new List<SluiceClass>();
            SpillCollection = new List<SpillClass>();
            CrumpCollection = new List<CrumpClass>();
            GatedWeirCollection = new List<GatedWeirClass>();
            NotionalWeirCollection = new List<NotionalWeirClass>();
            FlatVWeirCollection = new List<FlatVWeirClass>();
            QHControlCollection = new List<QHControlClass>();
            RoundNoseBroadCrestedWeirCollection = new List<RoundNosedBroadCrestedWeirClass>();
            LabyrinthWeirCollection = new List<LabyrinthWeirClass>();
            SharpCrestedWeirCollection = new List<SharpCrestedWeirClass>();
            SyphonCollection = new List<SyphonClass>();
            WeirCollection = new List<WeirClass>();
            // WIND

            RiverReachCollection = new List<RiverReachClass>();
            MIKE11StructureCollection = new List<MIKE11StructureClass>();
            MIKE11BoundaryCollection = new List<MIKE11BoundaryClass>();
        }

        public Action<int, string> SetStatus;
        public void ReadGeometryFile(string geofile)
        {
            if (SetStatus == null)
                return;

            inputFilestringarray = System.IO.File.ReadAllLines(geofile);

            SetStatus(8, "");

            HydraulicElementsCollection = new HydraulicElementGeoCollectionClass(inputFilestringarray);
            int N = HydraulicElementsCollection.ElementList.Count;
            string Nstr = N.ToString();

            SetStatus(20, "");
        }

        public bool FindRiverLocation(string label, ref string rivername, ref double chainage)
        {

            for (int i = 0; (i < RiverReachCollection.Count); i++)
            {
                RiverReachClass lReach = RiverReachCollection[i];

                double chain = lReach.StartChainage;
                for (int ii = 0; (ii < lReach.XsecCollection.Count); ii++)
                {
                    if (lReach.XsecCollection[ii].Label[0] == label)
                    {
                        rivername = lReach.Name;
                        chainage = chain;
                        return true;
                    }
                    chain = chain + lReach.XsecCollection[ii].deltaChainage;

                }

            }
            return false;

        }

        private void HandleRiver(ref int i, string[] Filestringarray, ref double DeltaChain, ref int RiverNo, string Keyword)
        {

            RiverReachClass LRiverreach;
            string Keyword2 = "";
            if (DeltaChain == 0 /*|| !IsRiver*/)
            {
                RiverNo++;
                string Name = "River " + RiverNo.ToString();
                LRiverreach = new RiverReachClass(Name);
                RiverReachCollection.Add(LRiverreach);
            }
            else
            {
                LRiverreach = RiverReachCollection[RiverReachCollection.Count - 1];
            }

            switch (Keyword)
            {
                case "RIVER":
                    {
                        i++;
                        string test = inputFilestringarray[i].PadRight(inputFilestringarray[i].Length + 1);
                        int IndexOfFirstSpace = test.IndexOf(" ");
                        Keyword2 = inputFilestringarray[i].Substring(0, IndexOfFirstSpace);
                        i--;
                        switch (Keyword2)
                        {
                            case "SECTION":
                                {
                                    CrossSectionClass lxsec = new CrossSectionClass(Keyword, inputFilestringarray, ref i, ref errLineNums);
                                    DeltaChain = lxsec.deltaChainage;
                                    LRiverreach.XsecCollection.Add(lxsec);
                                    break;
                                }
                            case "MUSKINGUM":
                                {
                                    i = MuskingumCollection.add(i, inputFilestringarray, ref DeltaChain);
                                    break;

                                }
                            default:
                                break;
                        }
                        break;
                    }
                case "CONDUIT":
                    {
                        ConduitClass Lconduit = new ConduitClass(Keyword, inputFilestringarray, ref i, ref errLineNums);
                        DeltaChain = Lconduit.deltaChainage;
                        LRiverreach.XsecCollection.Add(Lconduit);
                        break;
                    }
                case "INTERPOLATE":
                    {
                        SectionBaseClass lxsec;
                        if (LRiverreach.XsecCollection[LRiverreach.XsecCollection.Count - 1].Keyword == "RIVER")
                        {
                            lxsec = new CrossSectionClass(Keyword, inputFilestringarray, ref i, ref errLineNums);
                        }
                        else
                        {
                            lxsec = new ConduitClass(Keyword, inputFilestringarray, ref i, ref errLineNums);
                        }
                        DeltaChain = lxsec.deltaChainage;
                        lxsec.Interpolate = true;
                        LRiverreach.XsecCollection.Add(lxsec);
                        break;
                    }
                case "REPLICATE":
                    {
                        if (LRiverreach.XsecCollection[LRiverreach.XsecCollection.Count - 1].Keyword == "RIVER")
                        {
                            LineReaderClass l = new LineReaderClass();
                            i++;
                            i++;
                            bool OK = false;
                            double delChain = l.GetDouble(inputFilestringarray[i], 1, i, ref OK, ref errLineNums);
                            double delZ = l.GetDouble(inputFilestringarray[i], 2, i, ref OK, ref errLineNums);
                            CrossSectionClass PreceedingXsec = LRiverreach.XsecCollection[LRiverreach.XsecCollection.Count - 1] as CrossSectionClass;
                            CrossSectionClass lxsec = new CrossSectionClass(PreceedingXsec, delChain, delZ);
                            DeltaChain = lxsec.deltaChainage; i--;
                            i--;
                            lxsec.Comment = l.GetComment(Keyword, inputFilestringarray[i]);
                            i++;
                            lxsec.GetLabels(inputFilestringarray[i]);
                            lxsec.copy = true;
                            LRiverreach.XsecCollection.Add(lxsec);
                            i++;
                        }
                        if (LRiverreach.XsecCollection[LRiverreach.XsecCollection.Count - 1].Keyword == "CONDUIT")
                        {
                            LineReaderClass l = new LineReaderClass();
                            i++;
                            i++;
                            bool OK = false;
                            double delChain = l.GetDouble(inputFilestringarray[i], 1, i, ref OK, ref errLineNums);
                            double delZ = l.GetDouble(inputFilestringarray[i], 2, i, ref OK, ref errLineNums);
                            ConduitClass PreceedingXsec = LRiverreach.XsecCollection[LRiverreach.XsecCollection.Count - 1] as ConduitClass;
                            ConduitClass lxsec = new ConduitClass(PreceedingXsec, delChain, delZ);
                            DeltaChain = lxsec.deltaChainage;
                            i--;
                            i--;
                            lxsec.Comment = l.GetComment(Keyword, inputFilestringarray[i]);
                            i++;
                            lxsec.GetLabels(inputFilestringarray[i]);
                            lxsec.copy = true;
                            LRiverreach.XsecCollection.Add(lxsec);
                            i++;
                        }
                        break;
                    }

            }

        }
        public bool ReadDataFile(string datafile, out string errMsg)
        {
            bool res = false;
            errMsg = string.Empty;
            if (SetStatus == null)
            {
                errMsg = "Status progress not initialized";
                return res;
            }
            inputFilestringarray = System.IO.File.ReadAllLines(datafile);
            int MaxNoLines = inputFilestringarray.Count(); // input file stored in an overall string array. An item per line in file

            if (inputFilestringarray[1].Contains("#REVISION#"))
            {
                LineReaderClass l = new LineReaderClass();
                bool ok = true;
                LineReaderClass.labelLength = l.GetInt(inputFilestringarray[2], 6, 2, ref ok, ref errLineNums);
            }


            double DeltaChain = 0;

            int i = 0;
            int RiverNo = 0;
            bool OKtoRead = true;

            List<int> record = new List<int>();
            int step = MaxNoLines / 20;
            try
            {
                while (i < MaxNoLines)
                {
                    int temp = MaxNoLines % step;
                    if (!record.Contains(temp))
                    {
                        record.Add(temp);
                        SetStatus(20 + temp, "");
                    }
                    string test = inputFilestringarray[i].PadRight(inputFilestringarray[i].Length + 1);
                    int IndexOfFirstSpace = test.IndexOf(" ");
                    string Keyword = " ";
                    string Keyword2 = "";
                    if (IndexOfFirstSpace > -1)
                        Keyword = inputFilestringarray[i].Substring(0, IndexOfFirstSpace);
                    if (Keyword.Contains("***"))
                        if (OKtoRead) { OKtoRead = false; } else { OKtoRead = true; }; // skip dividing sections
                                                                                       //Keyword = Keyword.ToUpper();

                    if (OKtoRead)
                    {
                        switch (Keyword)
                        {

                            case "ABSTRACTION":
                                {
                                    IsRiver = false;

                                    AbstractionClass LAbstraction = new AbstractionClass(inputFilestringarray, ref i, ref errLineNums);
                                    AbstractionCollection.Add(LAbstraction);
                                    break;
                                }
                            case "BERNOULLI":
                                {
                                    IsRiver = false;

                                    BernoulliLossClass LBernoulliLoss = new BernoulliLossClass(inputFilestringarray, ref i, ref errLineNums);
                                    LBernoulliLoss.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.BERNOULLI_LOSS, HydraulicElementsCollection.ElementList);
                                    BernoulliLossCollection.Add(LBernoulliLoss);
                                    StructureCollection.Add(LBernoulliLoss);
                                    break;
                                }
                            case "BLOCKAGE":
                                {
                                    IsRiver = false;

                                    BlockageClass LBlockage = new BlockageClass(inputFilestringarray, ref i, ref errLineNums);
                                    LBlockage.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.BLOCKAGE, HydraulicElementsCollection.ElementList);
                                    BlockageCollection.Add(LBlockage);
                                    StructureCollection.Add(LBlockage);
                                    break;
                                }
                            // Boundaries start
                            case "QHBDY":
                                {
                                    IsRiver = false;

                                    QHBoundaryClass LQHBoundary = new QHBoundaryClass(inputFilestringarray, ref i, ref errLineNums);
                                    QHBoundaryCollection.Add(LQHBoundary);
                                    break;
                                }
                            case "QTBDY":
                                {
                                    IsRiver = false;

                                    QtBoundaryClass LQtBoundary = new QtBoundaryClass(inputFilestringarray, ref i, ref errLineNums);
                                    QtBoundaryCollection.Add(LQtBoundary);
                                    break;

                                }
                            case "HTBDY":
                                {
                                    IsRiver = false;

                                    HtBoundaryClass LHtBoundary = new HtBoundaryClass(inputFilestringarray, ref i, ref errLineNums);
                                    HtBoundaryCollection.Add(LHtBoundary);
                                    break;

                                }
                            case "TIDBDY":
                                {
                                    IsRiver = false;

                                    TidalBoundaryClass LTidalBoundary = new TidalBoundaryClass(inputFilestringarray, ref i, ref errLineNums);
                                    TidalBoundaryCollection.Add(LTidalBoundary);
                                    break;

                                }
                            case "REBDY":
                                {
                                    IsRiver = false;

                                    RainEvapInfiltrationClass LRainEvapInfiltrationBoundary = new RainEvapInfiltrationClass(inputFilestringarray, ref i, ref errLineNums);
                                    RainEvapInfiltrationCollection.Add(LRainEvapInfiltrationBoundary);
                                    break;

                                }
                            case "NCDBDY":
                                {
                                    IsRiver = false;

                                    NormalCriticalBoundaryClass LNormalCriticalBoundary = new NormalCriticalBoundaryClass(inputFilestringarray, ref i, ref errLineNums);
                                    NormalCriticalBoundaryCollection.Add(LNormalCriticalBoundary);
                                    break;

                                }
                            // Boundaries end

                            case "BRIDGE":
                                {
                                    IsRiver = false;

                                    i++;
                                    test = inputFilestringarray[i].PadRight(inputFilestringarray[i].Length + 1);
                                    IndexOfFirstSpace = test.IndexOf(" ");
                                    if (IndexOfFirstSpace > -1)
                                        Keyword2 = inputFilestringarray[i].Substring(0, IndexOfFirstSpace);
                                    i--;
                                    BridgeClass LBridge;
                                    if (Keyword2 == "USBPR1978")
                                    {

                                        LBridge = new USBPRBridgeClass(inputFilestringarray, ref i, ref errLineNums);
                                        LBridge.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.BRIDGE_USBPR1978, HydraulicElementsCollection.ElementList);

                                    }
                                    else
                                    {
                                        LBridge = new ArchBridgeClass(inputFilestringarray, ref i, ref errLineNums);
                                        LBridge.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.BRIDGE_ARCH, HydraulicElementsCollection.ElementList);

                                    }
                                    BridgeCollection.Add(LBridge);
                                    StructureCollection.Add(LBridge);
                                    break;

                                }
                            case "COMMENT":
                                {
                                    IsRiver = false;
                                    i++;
                                    LineReaderClass l = new LineReaderClass();
                                    bool ok = false;
                                    int NoToskip = l.GetInt(inputFilestringarray[i], 1, i, ref ok, ref errLineNums);
                                    if (ok) i = i + NoToskip;
                                    // ignored
                                    break;
                                }
                            case "CONDUIT":
                                {
                                    HandleRiver(ref i, inputFilestringarray, ref DeltaChain, ref RiverNo, Keyword);
                                    IsRiver = true;
                                    break;
                                }
                            case "CULVERT":
                                {
                                    IsRiver = false;

                                    i++;
                                    test = inputFilestringarray[i].PadRight(inputFilestringarray[i].Length + 1);
                                    IndexOfFirstSpace = test.IndexOf(" ");
                                    if (IndexOfFirstSpace > -1)
                                        Keyword2 = inputFilestringarray[i].Substring(0, IndexOfFirstSpace);
                                    i--;
                                    CulvertClass LCulvert;
                                    switch (Keyword2)
                                    {
                                        case "INLET":
                                            {

                                                LCulvert = new CulvertInletClass(inputFilestringarray, ref i, ref errLineNums);
                                                LCulvert.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.CULVERT_INLET, HydraulicElementsCollection.ElementList);

                                                break;
                                            }
                                        case "OUTLET":
                                            {
                                                LCulvert = new CulvertOutletClass(inputFilestringarray, ref i, ref errLineNums);
                                                LCulvert.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.CULVERT_OUTLET, HydraulicElementsCollection.ElementList);

                                                break;
                                            }

                                        case "BEND":
                                            {
                                                LCulvert = new CulvertBendClass(inputFilestringarray, ref i, ref errLineNums);
                                                LCulvert.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.CULVERT_BEND, HydraulicElementsCollection.ElementList);

                                                break;
                                            }
                                        default:
                                            {
                                                LCulvert = new CulvertClass(inputFilestringarray, ref i);
                                                break;
                                            }
                                    }

                                    CulvertCollection.Add(LCulvert);
                                    StructureCollection.Add(LCulvert);
                                    break;

                                }
                            case "FLOODPLAIN":
                                {
                                    IsRiver = false;

                                    FloodPlainClass LFloodPlain = new FloodPlainClass(inputFilestringarray, ref i, ref errLineNums);
                                    FloodPlainCollection.Add(LFloodPlain);
                                    break;

                                }
                            case "GAUGE":
                                {
                                    // ignored
                                    IsRiver = false;
                                    break;
                                }
                            case "LOSS":
                                {
                                    IsRiver = false;
                                    // general head loss
                                    HeadLossClass LHeadLoss = new HeadLossClass(inputFilestringarray, ref i, ref errLineNums);
                                    LHeadLoss.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.LOSS, HydraulicElementsCollection.ElementList);

                                    HeadLossCollection.Add(LHeadLoss);
                                    StructureCollection.Add(LHeadLoss);
                                    break;
                                }
                            case "FSSR16BDY":
                                {
                                    // ignored
                                    IsRiver = false;
                                    break;
                                }
                            case "SCSBDY":
                                {
                                    // ignored
                                    IsRiver = false;
                                    break;
                                }
                            case "FEHBDY":
                                {
                                    // ignored
                                    IsRiver = false;
                                    break;
                                }
                            case "REFHBDY":
                                {
                                    // ignored
                                    IsRiver = false;
                                    break;
                                }
                            case "INTERPOLATE":
                                {
                                    HandleRiver(ref i, inputFilestringarray, ref DeltaChain, ref RiverNo, Keyword);
                                    IsRiver = true;
                                    break;
                                }
                            case "JUNCTION":
                                {
                                    IsRiver = false;
                                    JunctionClass LJunction = new JunctionClass(inputFilestringarray, ref i);
                                    LJunction.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.JUNCTION_OPEN, HydraulicElementsCollection.ElementList);
                                    JunctionCollection.Add(LJunction);
                                    StructureCollection.Add(LJunction);
                                    break;
                                }
                            case "LATERAL":
                                {
                                    IsRiver = false;
                                    LateralClass LLateral = new LateralClass(inputFilestringarray, ref i, ref errLineNums);
                                    LateralCollection.Add(LLateral);
                                    break;
                                }
                            case "MANHOLE":
                                {
                                    IsRiver = false;
                                    ManholeClass LManhole = new ManholeClass(inputFilestringarray, ref i, ref errLineNums);
                                    LManhole.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.MANHOLE, HydraulicElementsCollection.ElementList);
                                    ManholeCollection.Add(LManhole);
                                    StructureCollection.Add(LManhole);
                                    break;
                                }
                            case "ORIFICE":
                                {
                                    IsRiver = false;
                                    OrificeClass LOrifice = new OrificeClass(inputFilestringarray, ref i, ref errLineNums);
                                    if (LOrifice.flapped)
                                        LOrifice.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.ORIFICE_FLAPPED, HydraulicElementsCollection.ElementList);
                                    else
                                        LOrifice.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.ORIFICE_OPEN, HydraulicElementsCollection.ElementList);

                                    OrificeCollection.Add(LOrifice);
                                    StructureCollection.Add(LOrifice);
                                    break;
                                }
                            case "POND":
                                {
                                    IsRiver = false;
                                    PondClass LPond = new PondClass(inputFilestringarray, ref i, ref errLineNums);
                                    LPond.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.POND, HydraulicElementsCollection.ElementList);

                                    PondCollection.Add(LPond);
                                    StructureCollection.Add(LPond);
                                    break;
                                }
                            case "OCPUMP":
                                {
                                    IsRiver = false;
                                    PumpClass LPump = new PumpClass(inputFilestringarray, ref i, ref errLineNums);
                                    LPump.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.OCPUMP, HydraulicElementsCollection.ElementList);

                                    PumpCollection.Add(LPump);
                                    StructureCollection.Add(LPump);
                                    break;
                                }
                            case "QRATING":
                                {
                                    IsRiver = false;
                                    RatingCurveClass LQH = new RatingCurveClass(inputFilestringarray, ref i, ref errLineNums);
                                    RatingCurveCollection.Add(LQH);
                                    break;
                                }
                            case "REPLICATE":
                                {
                                    HandleRiver(ref i, inputFilestringarray, ref DeltaChain, ref RiverNo, Keyword);
                                    IsRiver = true;
                                    break;
                                }
                            case "RESERVOIR":
                                {
                                    IsRiver = false;
                                    ReservoirClass LReservoir = new ReservoirClass(inputFilestringarray, ref i, ref errLineNums);
                                    ReservoirCollection.Add(LReservoir);
                                    break;
                                }
                            case "RIVER":
                                {
                                    HandleRiver(ref i, inputFilestringarray, ref DeltaChain, ref RiverNo, Keyword);
                                    IsRiver = true;
                                    break;
                                }
                            case "RULES":
                                {
                                    IsRiver = false;
                                    // ignored
                                    break;
                                }
                            case "SLUICE":
                                {
                                    IsRiver = false;
                                    SluiceClass LSluice = new SluiceClass(inputFilestringarray, ref i, ref errLineNums);
                                    if (LSluice.SluiceType == SluiceClass.SluiceTypes.Radial)
                                        LSluice.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.SLUICE_RADIAL, HydraulicElementsCollection.ElementList);
                                    else
                                        LSluice.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.SLUICE_VERTICAL, HydraulicElementsCollection.ElementList);

                                    SluiceCollection.Add(LSluice);
                                    StructureCollection.Add(LSluice);
                                    break;
                                }
                            case "SPILL":
                                {
                                    IsRiver = false;
                                    SpillClass LSpill = new SpillClass(inputFilestringarray, ref i, ref errLineNums);
                                    LSpill.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.SPILL, HydraulicElementsCollection.ElementList);
                                    SpillCollection.Add(LSpill);
                                    break;
                                }
                            case "CRUMP":
                                {
                                    IsRiver = false;
                                    CrumpClass LCrump = new CrumpClass(inputFilestringarray, ref i, ref errLineNums);
                                    LCrump.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.CRUMP, HydraulicElementsCollection.ElementList);

                                    CrumpCollection.Add(LCrump);
                                    StructureCollection.Add(LCrump);
                                    break;
                                }
                            case "GATED":
                                {
                                    IsRiver = false;
                                    GatedWeirClass LGatedWeir = new GatedWeirClass(inputFilestringarray, ref i, ref errLineNums);
                                    LGatedWeir.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.GATED_WEIR, HydraulicElementsCollection.ElementList);

                                    GatedWeirCollection.Add(LGatedWeir);
                                    StructureCollection.Add(LGatedWeir);
                                    break;
                                }
                            case "NOTWEIR":
                                {
                                    IsRiver = false;
                                    NotionalWeirClass LNotionalWeir = new NotionalWeirClass(inputFilestringarray, ref i, ref errLineNums);
                                    LNotionalWeir.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.NOTWEIR, HydraulicElementsCollection.ElementList);

                                    NotionalWeirCollection.Add(LNotionalWeir);
                                    StructureCollection.Add(LNotionalWeir);
                                    break;
                                }
                            case "FLAT-V":
                                {
                                    IsRiver = false;
                                    FlatVWeirClass LFlatVWeir = new FlatVWeirClass(inputFilestringarray, ref i, ref errLineNums);
                                    LFlatVWeir.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.NOTDEFINED, HydraulicElementsCollection.ElementList);

                                    FlatVWeirCollection.Add(LFlatVWeir);
                                    StructureCollection.Add(LFlatVWeir);
                                    break;
                                }
                            case "QH":
                                {
                                    IsRiver = false;
                                    QHControlClass LQHControl = new QHControlClass(inputFilestringarray, ref i, ref errLineNums);
                                    QHControlCollection.Add(LQHControl);
                                    StructureCollection.Add(LQHControl);
                                    break;
                                }
                            case "RNWEIR":
                                {
                                    IsRiver = false;
                                    RoundNosedBroadCrestedWeirClass LRoundNoseBroadCrestedWeir = new RoundNosedBroadCrestedWeirClass(inputFilestringarray, ref i, ref errLineNums);
                                    LRoundNoseBroadCrestedWeir.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.RNWEIR, HydraulicElementsCollection.ElementList);
                                    RoundNoseBroadCrestedWeirCollection.Add(LRoundNoseBroadCrestedWeir);
                                    StructureCollection.Add(LRoundNoseBroadCrestedWeir);
                                    break;
                                }
                            case "LABYRINTH":
                                {
                                    IsRiver = false;
                                    LabyrinthWeirClass LLabyrinthWeir = new LabyrinthWeirClass(inputFilestringarray, ref i, ref errLineNums);
                                    LLabyrinthWeir.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.NOTDEFINED, HydraulicElementsCollection.ElementList);
                                    LabyrinthWeirCollection.Add(LLabyrinthWeir);
                                    StructureCollection.Add(LLabyrinthWeir);
                                    break;
                                }
                            case "SCWEIR":
                                {
                                    IsRiver = false;
                                    SharpCrestedWeirClass LSharpCrestedWeir = new SharpCrestedWeirClass(inputFilestringarray, ref i, ref errLineNums);
                                    LSharpCrestedWeir.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.SCWEIR, HydraulicElementsCollection.ElementList);

                                    SharpCrestedWeirCollection.Add(LSharpCrestedWeir);
                                    StructureCollection.Add(LSharpCrestedWeir);
                                    break;
                                }
                            case "SYPHON":
                                {
                                    IsRiver = false;
                                    SyphonClass LSyphon = new SyphonClass(inputFilestringarray, ref i, ref errLineNums);
                                    LSyphon.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.SYPHON, HydraulicElementsCollection.ElementList);

                                    SyphonCollection.Add(LSyphon);
                                    StructureCollection.Add(LSyphon);
                                    break;
                                }
                            case "WEIR":
                                {
                                    IsRiver = false;
                                    WeirClass LWeir = new WeirClass(inputFilestringarray, ref i, ref errLineNums);
                                    LWeir.SetGeoPoint(HydraulicElementsGeoClass.ElementTypes.WEIR, HydraulicElementsCollection.ElementList);

                                    WeirCollection.Add(LWeir);
                                    StructureCollection.Add(LWeir);
                                    break;
                                }
                            case "GISINFO":
                                {
                                    IsRiver = false;

                                    i = MaxNoLines; // bail out no more info
                                    break;
                                }
                            default:
                                {
                                    IsRiver = false;
                                    break;
                                }
                        }
                    }
                    i++;
                }

                SetStatus(40, "");
                res = true;
            }
            catch (Exception e)
            {
                SetStatus(0, "Error");
                errMsg = String.Format("Input string at line {0} is not in a correct format. The string is '{1}'," +
                    "but it doesn't match the expected columns width.", (i + 1).ToString(), inputFilestringarray[i]);
                res = false;
            }

            return res;
        }

        public void WriteXNS11(string XNS11)
        {
            if (SetStatus == null)
                return;
            ExportXNSClass Exporter = new ExportXNSClass();
            foreach (RiverReachClass Lreach in RiverReachCollection)
            {
                Exporter.CreateReachCrossSections(Lreach);
            }
            Exporter.Save(XNS11);
            SetStatus(75, "");
        }
        public void CreateNetwork()
        {
            if (SetStatus == null)
                return;

            for (int i = 0; (i < RiverReachCollection.Count); i++)
            {
                RiverReachClass lReach = RiverReachCollection[i];
                lReach.AddGeoinformation(HydraulicElementsCollection);
                lReach.XsecGeoAutoMatch();
            }

            SetStatus(45, "");

            if (HydraulicElementsCollection.ElementList.Count > 0)
            {
                StructureClass.SetJunctionRiver(RiverReachCollection, StructureCollection);
                StructureClass.CreateAllBranch(ref RiverReachCollection, StructureCollection);
                StructureClass.SetJuncriverStructure(RiverReachCollection, StructureCollection);

                SetStatus(51, "");

                int step = StructureCollection.Count / 7;
                for (int i = 0; (i < StructureCollection.Count); i++)
                {
                    int temp = 51 + StructureCollection.Count % 7;
                    SetStatus(temp, "");

                    StructureClass lstruc = StructureCollection[i];
                    lstruc.SetAllConnections(ref RiverReachCollection, StructureCollection);
                }

                SetStatus(58, "");

                for (int i = 0; (i < StructureCollection.Count); i++)
                {
                    int temp = 58 + StructureCollection.Count % 5;
                    SetStatus(temp, "");

                    StructureClass lstruc = StructureCollection[i];
                    lstruc.SetJunConnections(ref RiverReachCollection, StructureCollection);
                }

                SetStatus(63, "");

                for (int i = 0; (i < JunctionCollection.Count); i++)
                {
                    JunctionClass lJunc = JunctionCollection[i];
                }

                for (int i = 0; (i < RiverReachCollection.Count); i++)
                {
                    RiverReachClass lReach = RiverReachCollection[i];
                    lReach.FixCrossSections(RiverReachCollection);
                }

                SetStatus(65, "");

                for (int i = 0; (i < StructureCollection.Count); i++)
                {
                    StructureClass lstruc = StructureCollection[i];
                    MIKE11StructureClass MIKE11Structure = lstruc.CreateMIKE11Structure(lstruc);
                    MIKE11StructureCollection.Add(MIKE11Structure);
                }

                SetStatus(66, "");

                for (int i = 0; (i < HtBoundaryCollection.Count); i++)
                {
                    HtBoundaryClass lHTbound = HtBoundaryCollection[i];
                    string rivername = "";
                    double chain = 0;
                    if (lHTbound.ID != null)
                    {
                        if (FindRiverLocation(lHTbound.ID.Labels[0], ref rivername, ref chain))
                        {
                            MIKE11WaterLevelBoundaryClass lM11HTBND = new MIKE11WaterLevelBoundaryClass(rivername, chain, lHTbound);
                            MIKE11BoundaryCollection.Add(lM11HTBND);

                        }
                    }

                }

                SetStatus(67, "");

                for (int i = 0; (i < QtBoundaryCollection.Count); i++)
                {
                    QtBoundaryClass lQTbound = QtBoundaryCollection[i];
                    string rivername = "";
                    double chain = 0;

                    if (lQTbound.ID != null)
                    {
                        if (FindRiverLocation(lQTbound.ID.Labels[0], ref rivername, ref chain))
                        {

                            MIKE11DischargeBoundaryClass lM11QTBND = new MIKE11DischargeBoundaryClass(rivername, chain, lQTbound);
                            MIKE11BoundaryCollection.Add(lM11QTBND);

                        }
                    }

                }

                SetStatus(68, "");

                for (int i = 0; (i < QHBoundaryCollection.Count); i++)
                {
                    QHBoundaryClass lQHbound = QHBoundaryCollection[i];
                    string rivername = "";
                    double chain = 0;

                    if (lQHbound.ID != null)
                    {
                        if (FindRiverLocation(lQHbound.ID.Labels[0], ref rivername, ref chain))
                        {
                            MIKE11QHBoundaryClass lM11QHBND = new MIKE11QHBoundaryClass(rivername, chain, lQHbound);
                            MIKE11BoundaryCollection.Add(lM11QHBND);

                        }
                    }
                }

            }
            SetStatus(70, "");
        }

        public void ExportBoundary(string FolderName, DHI.MHydro.Data.RootPfs MHData)
        {
            foreach (MIKE11BoundaryClass item in MIKE11BoundaryCollection)
            {
                MIKE11WaterLevelBoundaryClass itemWaterLevel = null;
                MIKE11DischargeBoundaryClass itemDischarge = null;
                MIKE11QHBoundaryClass itemQH = null;

                itemWaterLevel = item as MIKE11WaterLevelBoundaryClass;
                itemDischarge = item as MIKE11DischargeBoundaryClass;
                itemQH = item as MIKE11QHBoundaryClass;

                string path = "";
                if (itemWaterLevel != null)
                {
                    path = FolderName + itemWaterLevel.dfs0Filename;
                    //dfs0 file
                    CreateDfs0FileISIS.CreateDfs0NeqCalWaterlevel(path, true, itemWaterLevel.TimeSeries);
                    //mhydro data
                    BoundaryPfs boundPfs = new BoundaryPfs(MHData);
                    boundPfs.BoundaryType = EnumBoundaryType.WaterLevel;
                    boundPfs.Name = itemWaterLevel.ID;
                    boundPfs.BranchName = itemWaterLevel.RiverName;
                    if (nameGuidDict.ContainsKey(itemWaterLevel.RiverName))
                    {
                        boundPfs.BranchID = nameGuidDict[itemWaterLevel.RiverName];
                    }
                    boundPfs.Chainage = itemWaterLevel.Chainage;
                    boundPfs.ReCalculateShape();
                    boundPfs.HDInputType = EnumTemporalType.TimeVarying;
                    Int32[] selectedItems = { 1 };
                    DhiSeDfsSelection dfs = new DhiSeDfsSelection(path, selectedItems);
                    boundPfs.HDTS.Assign(dfs);
                    MHData.BoundaryList.Add(boundPfs);
                }
                if (itemDischarge != null)
                {
                    path = FolderName + itemDischarge.dfs0Filename;
                    //dfs0 file
                    CreateDfs0FileISIS.CreateDfs0NeqCalDischarge(path, true, itemDischarge.TimeSeries);
                    //mhydro data
                    BoundaryPfs boundPfs = new BoundaryPfs(MHData);
                    boundPfs.BoundaryType = EnumBoundaryType.Discharge;
                    boundPfs.Name = itemDischarge.ID;
                    boundPfs.BranchName = itemDischarge.RiverName;
                    if (nameGuidDict.ContainsKey(itemDischarge.RiverName))
                    {
                        boundPfs.BranchID = nameGuidDict[itemDischarge.RiverName];
                    }
                    boundPfs.Chainage = itemDischarge.Chainage;
                    boundPfs.ReCalculateShape();
                    boundPfs.HDInputType = EnumTemporalType.TimeVarying;
                    Int32[] selectedItems = { 1 };
                    DhiSeDfsSelection dfs = new DhiSeDfsSelection(path, selectedItems);
                    boundPfs.HDTS.Assign(dfs);
                    MHData.BoundaryList.Add(boundPfs);
                }
                if (itemQH != null)
                {
                    //CreateDfs0FileISIS.CreateDfs0File2(itemQH.dfs0Filename, true);
                }
            }
        }

        public void WriteMIKEHydroFile(string FileName, string CRSFileName, string BoundaryFolder)
        {
            if (SetStatus == null)
                return;

            DHI.MHydro.Data.RootPfs MHData = null;
            if (rootPfs == null)
            {
                MHData = new DHI.MHydro.Data.RootPfs();
            }
            else
            {
                MHData = rootPfs;
            }

            MHData.UndoMgr.Disable();

            try
            {
                SetStatus(76, "");

                Import_NetWorkData(MHData);

                SetStatus(82, "");

                MHData.CrossSectionList.FilePath = CRSFileName;
                MHData.CrossSectionList.LoadFromFile();

                SetStatus(84, "");

                //Structures
                Import_MIKE11ControlStructure(MHData);
                SetStatus(85, "");

                Import_MIKE11Culvert(MHData);
                SetStatus(86, "");

                Import_MIKE11Energyloss(MHData);
                SetStatus(87, "");

                Import_MIKE11Pump(MHData);
                SetStatus(88, "");

                Import_MIKE11TabulatedStructure(MHData);
                SetStatus(89, "");

                Import_MIKE11Weir(MHData);
                SetStatus(90, "");

                //Linkchannel
                Import_LinkChannel(MHData);
                SetStatus(93, "");

                //Simulation period and timestep
                Import_SimulationPeriodAndTimestep(MHData);
                SetStatus(94, "");

                //boundary condition
                ExportBoundary(BoundaryFolder, MHData);
                SetStatus(96, "");

                MHData.Save(FileName);
                SetStatus(98, "");
            }
            finally
            {
                MHData.UndoMgr.Enable();
                MHData.EnableUndo(true);
            }
        }

        private void Import_SimulationPeriodAndTimestep(DHI.MHydro.Data.RootPfs MHData)
        {
            MHData.SimulationPeriod.StartTime = datetimeStart;
            DateTime datetimeEnd = new DateTime();
            datetimeEnd = datetimeStart.AddHours(duration);
            MHData.SimulationPeriod.EndTime = datetimeEnd;

            MHData.SimulationTimeStep.LengthType = EnumSimulationTimeStepLengthType_RR.Seconds;
            MHData.SimulationTimeStep.TimeStep = timestep;
        }

        #region Structures
        private void Import_MIKE11ControlStructure(DHI.MHydro.Data.RootPfs MHData)
        {
            foreach (MIKE11StructureClass item in MIKE11StructureCollection)
            {
                MIKE11ControlStructureClass itemInherit = null;
                itemInherit = item as MIKE11ControlStructureClass;
                if (itemInherit != null)
                {
                    DHI.MHydro.Data.GatePfs gatePfs = null;
                    gatePfs = new DHI.MHydro.Data.GatePfs(MHData);

                    gatePfs.ID = itemInherit.ID;
                    gatePfs.BranchName = itemInherit.RiverName;
                    if (nameGuidDict.ContainsKey(itemInherit.RiverName))
                    {
                        gatePfs.BranchID = nameGuidDict[itemInherit.RiverName];
                    }
                    gatePfs.Chainage = itemInherit.Chainage;
                    gatePfs.ReCalculateShape();

                    if (itemInherit.ControlStrucType == MIKE11ControlStructureClass.ControlStrucTypes.Overflow)
                    {
                        gatePfs.AttributeType = DHI.MHydro.Data.GateType.Overflow;
                    }
                    else if (itemInherit.ControlStrucType == MIKE11ControlStructureClass.ControlStrucTypes.Underflow)
                    {
                        gatePfs.AttributeType = DHI.MHydro.Data.GateType.Underflow;
                    }
                    else if (itemInherit.ControlStrucType == MIKE11ControlStructureClass.ControlStrucTypes.SluiceFormula)
                    {
                        gatePfs.AttributeType = DHI.MHydro.Data.GateType.SluiceGate;
                    }
                    else if (itemInherit.ControlStrucType == MIKE11ControlStructureClass.ControlStrucTypes.RadialGate)
                    {
                        gatePfs.AttributeType = DHI.MHydro.Data.GateType.RadialGate;
                    }
                    else
                    {
                        gatePfs.AttributeType = DHI.MHydro.Data.GateType.Overflow;
                    }

                    if (itemInherit.HeadLossFactorPositiveFlow != null)
                    {
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.Inflow))
                        {
                            gatePfs.PosInflow = itemInherit.HeadLossFactorPositiveFlow.Inflow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.OutFlow))
                        {

                            gatePfs.PosOutflow = itemInherit.HeadLossFactorPositiveFlow.OutFlow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.FreeFlow))
                        {
                            gatePfs.PosFreeOverflow = itemInherit.HeadLossFactorPositiveFlow.FreeFlow;
                        }
                    }

                    if (itemInherit.HeadLossFactorNegativeFlow != null)
                    {
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.Inflow))
                        {
                            gatePfs.NegInflow = itemInherit.HeadLossFactorNegativeFlow.Inflow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.OutFlow))
                        {

                            gatePfs.NegOutflow = itemInherit.HeadLossFactorNegativeFlow.OutFlow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.FreeFlow))
                        {
                            gatePfs.NegFreeOverflow = itemInherit.HeadLossFactorNegativeFlow.FreeFlow;
                        }
                    }

                    if (!double.IsInfinity(itemInherit.GateWidth))
                    {
                        gatePfs.GateWidth = itemInherit.GateWidth;
                    }
                    if (!double.IsInfinity(itemInherit.SillLevel))
                    {
                        gatePfs.SillLevel = itemInherit.SillLevel;
                    }
                    if (!double.IsInfinity(itemInherit.Height))
                    {
                        gatePfs.GateHeight = itemInherit.Height;
                    }
                    if (!double.IsInfinity(itemInherit.Radius))
                    {
                        gatePfs.Radius = itemInherit.Radius;
                    }
                    if (!double.IsInfinity(itemInherit.Trunnion))
                    {
                        gatePfs.Trunnion = itemInherit.Trunnion;
                    }
                    gatePfs.GatesNo = itemInherit.NoGates;


                    MHData.Structures.GateList.Add(gatePfs);
                }
            }
        }

        private void Import_MIKE11Culvert(DHI.MHydro.Data.RootPfs MHData)
        {
            foreach (MIKE11StructureClass item in MIKE11StructureCollection)
            {
                MIKE11CulvertClass itemInherit = null;
                itemInherit = item as MIKE11CulvertClass;
                if (itemInherit != null)
                {
                    DHI.MHydro.Data.CulvertPfs culvertPfs = null;
                    culvertPfs = new DHI.MHydro.Data.CulvertPfs(MHData);

                    culvertPfs.ID = itemInherit.ID;
                    culvertPfs.BranchName = itemInherit.RiverName;
                    if (nameGuidDict.ContainsKey(itemInherit.RiverName))
                    {
                        culvertPfs.BranchID = nameGuidDict[itemInherit.RiverName];
                    }
                    culvertPfs.Chainage = itemInherit.Chainage;
                    culvertPfs.ReCalculateShape();

                    if (itemInherit.Valve == MIKE11StructureClass.Valvesettings.NoFlow)
                    {
                        culvertPfs.ValveRegulation = ValveRegulation.NoFlow;
                    }
                    else if (itemInherit.Valve == MIKE11StructureClass.Valvesettings.none)
                    {
                        culvertPfs.ValveRegulation = ValveRegulation.None;
                    }
                    else if (itemInherit.Valve == MIKE11StructureClass.Valvesettings.OnlyNegative)
                    {
                        culvertPfs.ValveRegulation = ValveRegulation.OnlyNegativeFlow;
                    }
                    else if (itemInherit.Valve == MIKE11StructureClass.Valvesettings.OnlyPositive)
                    {
                        culvertPfs.ValveRegulation = ValveRegulation.OnlyPositiveFlow;
                    }

                    if (itemInherit.HeadLossFactorNegativeFlow != null)
                    {
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.Inflow))
                        {
                            culvertPfs.LNI = itemInherit.HeadLossFactorNegativeFlow.Inflow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.OutFlow))
                        {
                            culvertPfs.LNO = itemInherit.HeadLossFactorNegativeFlow.OutFlow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.FreeFlow))
                        {
                            culvertPfs.LNF = itemInherit.HeadLossFactorNegativeFlow.FreeFlow;
                        }
                    }

                    if (itemInherit.HeadLossFactorPositiveFlow != null)
                    {
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.Inflow))
                        {
                            culvertPfs.LPI = itemInherit.HeadLossFactorPositiveFlow.Inflow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.OutFlow))
                        {
                            culvertPfs.LPO = itemInherit.HeadLossFactorPositiveFlow.OutFlow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.FreeFlow))
                        {
                            culvertPfs.LPF = itemInherit.HeadLossFactorPositiveFlow.FreeFlow;
                        }
                    }

                    if (itemInherit.CulvertType == MIKE11CulvertClass.CulvertTypes.Circular)
                    {
                        culvertPfs.GeoType = DHI.MHydro.Data.GeoType.Circular;
                    }
                    else if (itemInherit.CulvertType == MIKE11CulvertClass.CulvertTypes.Rectangular)
                    {
                        culvertPfs.GeoType = DHI.MHydro.Data.GeoType.Rectangular;
                    }
                    else if (itemInherit.CulvertType == MIKE11CulvertClass.CulvertTypes.IrregularLeveldepth)
                    {
                        culvertPfs.GeoType = DHI.MHydro.Data.GeoType.Irregular;
                    }
                    else if (itemInherit.CulvertType == MIKE11CulvertClass.CulvertTypes.IrregularLevelWidth)
                    {
                        culvertPfs.GeoType = DHI.MHydro.Data.GeoType.Irregular;
                    }

                    if (!double.IsInfinity(itemInherit.Width))
                    {
                        culvertPfs.GeoWidth = itemInherit.Width;
                    }
                    if (!double.IsInfinity(itemInherit.Height))
                    {
                        culvertPfs.GeoHeight = itemInherit.Height;
                    }

                    if (!double.IsInfinity(itemInherit.Diameter))
                    {
                        culvertPfs.GeoDiameter = itemInherit.Diameter;
                    }
                    if (!double.IsInfinity(itemInherit.Length))
                    {
                        culvertPfs.Length = itemInherit.Length;
                    }

                    culvertPfs.NoCulvert = itemInherit.NoCulverts;
                    if (!double.IsInfinity(itemInherit.Manningsn))
                    {
                        culvertPfs.Manning = itemInherit.Manningsn;
                    }

                    MHData.Structures.CulvertList.Add(culvertPfs);
                }
            }
        }

        private void Import_MIKE11Energyloss(DHI.MHydro.Data.RootPfs MHData)
        {
            foreach (MIKE11StructureClass item in MIKE11StructureCollection)
            {
                MIKE11EnergyLossClass itemInherit = null;
                itemInherit = item as MIKE11EnergyLossClass;
                if (itemInherit != null)
                {
                    DHI.MHydro.Data.EnergyLossPfs energyPfs = null;
                    energyPfs = new DHI.MHydro.Data.EnergyLossPfs(MHData);

                    energyPfs.ID = itemInherit.ID;
                    energyPfs.BranchName = itemInherit.RiverName;
                    if (nameGuidDict.ContainsKey(itemInherit.RiverName))
                    {
                        energyPfs.BranchID = nameGuidDict[itemInherit.RiverName];
                    }
                    energyPfs.Chainage = itemInherit.Chainage;
                    energyPfs.ReCalculateShape();

                    if (itemInherit.Contraction != null)
                    {
                        if (!double.IsInfinity(itemInherit.Contraction.LossNeg))
                        {
                            energyPfs.ContractionNegFlow = itemInherit.Contraction.LossNeg;
                        }
                        if (!double.IsInfinity(itemInherit.Contraction.LossPos))
                        {
                            energyPfs.ContractionPosFlow = itemInherit.Contraction.LossPos;
                        }
                    }

                    if (itemInherit.Expansion != null)
                    {
                        if (!double.IsInfinity(itemInherit.Expansion.LossPos))
                        {
                            energyPfs.ExpansionPosFlow = itemInherit.Expansion.LossPos;
                        }
                        if (!double.IsInfinity(itemInherit.Expansion.LossNeg))
                        {
                            energyPfs.ExpansionNegFlow = itemInherit.Expansion.LossNeg;
                        }
                    }

                    if (itemInherit.Userdefined != null)
                    {
                        if (!double.IsInfinity(itemInherit.Userdefined.LossPos))
                        {
                            energyPfs.UserDefinedPosFlow = itemInherit.Userdefined.LossPos;
                        }
                        if (!double.IsInfinity(itemInherit.Userdefined.LossNeg))
                        {
                            energyPfs.UserDefinedNegFlow = itemInherit.Userdefined.LossNeg;
                        }
                    }

                    MHData.Structures.EnergyLossList.Add(energyPfs);
                }
            }
        }

        private void Import_MIKE11Pump(DHI.MHydro.Data.RootPfs MHData)
        {
            foreach (MIKE11StructureClass item in MIKE11StructureCollection)
            {
                MIKE11PumpClass itemInherit = null;
                itemInherit = item as MIKE11PumpClass;
                if (itemInherit != null)
                {
                    DHI.MHydro.Data.PumpPfs pumpPfs = null;
                    pumpPfs = new DHI.MHydro.Data.PumpPfs(MHData);

                    pumpPfs.ID = itemInherit.ID;
                    pumpPfs.BranchName = itemInherit.RiverName;
                    if (nameGuidDict.ContainsKey(itemInherit.RiverName))
                    {
                        pumpPfs.BranchID = nameGuidDict[itemInherit.RiverName];
                    }
                    pumpPfs.Chainage = itemInherit.Chainage;
                    pumpPfs.ReCalculateShape();

                    if (itemInherit.SpecificationType == MIKE11PumpClass.SpecificationTypes.FixedDischarge)
                    {
                        pumpPfs.PumpType = DHI.MHydro.Data.EnumPumpType.FixedDischarge;
                    }
                    else
                    {
                        pumpPfs.PumpType = DHI.MHydro.Data.EnumPumpType.Tabulated;
                    }

                    if (!double.IsInfinity(itemInherit.Discharge))
                    {
                        pumpPfs.Discharge = itemInherit.Discharge;
                    }
                    if (!double.IsInfinity(itemInherit.StartLevel))
                    {
                        pumpPfs.StartLevel = itemInherit.StartLevel;
                    }
                    if (!double.IsInfinity(itemInherit.StopLevel))
                    {
                        pumpPfs.StopLevel = itemInherit.StopLevel;
                    }
                    if (!double.IsInfinity(itemInherit.StartUpPeriode))
                    {
                        pumpPfs.StartPeriod = itemInherit.StartUpPeriode;
                    }
                    if (!double.IsInfinity(itemInherit.CloseDownPeriod))
                    {
                        pumpPfs.ClosePeriod = itemInherit.CloseDownPeriod;
                    }

                    MHData.Structures.PumpList.Add(pumpPfs);
                }
            }
        }

        private void Import_MIKE11TabulatedStructure(DHI.MHydro.Data.RootPfs MHData)
        {
            foreach (MIKE11StructureClass item in MIKE11StructureCollection)
            {
                MIKE11TabulatedStructureClass itemInherit = null;
                itemInherit = item as MIKE11TabulatedStructureClass;
                if (itemInherit != null)
                {
                    DHI.MHydro.Data.TabulatedPfs tabuPfs = null;
                    tabuPfs = new DHI.MHydro.Data.TabulatedPfs(MHData);

                    tabuPfs.ID = itemInherit.ID;
                    tabuPfs.BranchName = itemInherit.RiverName;
                    if (nameGuidDict.ContainsKey(itemInherit.RiverName))
                    {
                        tabuPfs.BranchID = nameGuidDict[itemInherit.RiverName];
                    }
                    tabuPfs.Chainage = itemInherit.Chainage;
                    tabuPfs.ReCalculateShape();

                    if (itemInherit.TabulatedType == MIKE11TabulatedStructureClass.TabulatedTypes.HdsHusQ)
                    {
                        tabuPfs.CalculationMode = DHI.MHydro.Data.TabulatedCalculationMode.HupsHdws;
                    }
                    else if (itemInherit.TabulatedType == MIKE11TabulatedStructureClass.TabulatedTypes.HusHdsQ)
                    {
                        tabuPfs.CalculationMode = DHI.MHydro.Data.TabulatedCalculationMode.HdwsQ;
                    }
                    else if (itemInherit.TabulatedType == MIKE11TabulatedStructureClass.TabulatedTypes.QHusHds)
                    {
                        tabuPfs.CalculationMode = DHI.MHydro.Data.TabulatedCalculationMode.HupsQ;
                    }

                    MHData.Structures.TabulatedList.Add(tabuPfs);
                }
            }
        }

        private void Import_MIKE11Weir(DHI.MHydro.Data.RootPfs MHData)
        {
            foreach (MIKE11StructureClass item in MIKE11StructureCollection)
            {
                MIKE11WeirClass itemInherit = null;
                itemInherit = item as MIKE11WeirClass;
                if (itemInherit != null)
                {
                    DHI.MHydro.Data.WeirPfs weirPfs = null;
                    weirPfs = new DHI.MHydro.Data.WeirPfs(MHData);

                    weirPfs.ID = itemInherit.ID;
                    weirPfs.BranchName = itemInherit.RiverName;
                    if (nameGuidDict.ContainsKey(itemInherit.RiverName))
                    {
                        weirPfs.BranchID = nameGuidDict[itemInherit.RiverName];
                    }
                    weirPfs.Chainage = itemInherit.Chainage;
                    weirPfs.ReCalculateShape();

                    if (itemInherit.WeirType == MIKE11WeirClass.WeirTypes.BroadCrestedWeir)
                    {
                        weirPfs.AttributeType = DHI.MHydro.Data.WeirType.BroadCrested;
                    }
                    else
                    {
                        weirPfs.AttributeType = DHI.MHydro.Data.WeirType.VillemonteFormula;
                    }

                    if (itemInherit.HeadLossFactorNegativeFlow != null)
                    {
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.Inflow))
                        {
                            weirPfs.LNI = itemInherit.HeadLossFactorNegativeFlow.Inflow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.OutFlow))
                        {
                            weirPfs.LNO = itemInherit.HeadLossFactorNegativeFlow.OutFlow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorNegativeFlow.FreeFlow))
                        {
                            weirPfs.LNF = itemInherit.HeadLossFactorNegativeFlow.FreeFlow;
                        }
                    }

                    if (itemInherit.HeadLossFactorPositiveFlow != null)
                    {
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.Inflow))
                        {
                            weirPfs.LPI = itemInherit.HeadLossFactorPositiveFlow.Inflow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.OutFlow))
                        {
                            weirPfs.LPO = itemInherit.HeadLossFactorPositiveFlow.OutFlow;
                        }
                        if (!double.IsInfinity(itemInherit.HeadLossFactorPositiveFlow.FreeFlow))
                        {
                            weirPfs.LPF = itemInherit.HeadLossFactorPositiveFlow.FreeFlow;
                        }
                    }

                    if (!double.IsInfinity(itemInherit.width))
                    {
                        weirPfs.Width = itemInherit.width;
                    }
                    if (!double.IsInfinity(itemInherit.Height))
                    {
                        weirPfs.Height = itemInherit.Height;
                    }

                    if (!double.IsInfinity(itemInherit.WeirCoeff))
                    {
                        weirPfs.WeirFormula.WeirCoeff = itemInherit.WeirCoeff;
                    }
                    if (!double.IsInfinity(itemInherit.WeirExp))
                    {
                        weirPfs.WeirFormula.WeirExp = itemInherit.WeirExp;
                    }
                    if (!double.IsInfinity(itemInherit.InvertLevel))
                    {
                        weirPfs.WeirFormula.InvertLevel = itemInherit.InvertLevel;
                    }

                    MHData.Structures.WeirList.Add(weirPfs);
                }
            }
        }
        #endregion

        private void Import_LinkChannel(DHI.MHydro.Data.RootPfs MHData)
        {
            MIKE11LinkChannelReachClass M11linkChannel = null;
            foreach (SpillClass spill in SpillCollection)
            {
                if (spill.ID.Labels.Count > 1)
                {
                    M11linkChannel = spill.MakeLinkChannel(RiverReachCollection, StructureCollection);
                }
                else
                {
                    continue;
                }
                if (M11linkChannel.GeoPoints.Count == 0)
                {
                    continue;
                }

                #region branch
                LineShape branchLineShape = new LineShape();
                foreach (GeoDigiPointClass coord in M11linkChannel.GeoPoints)
                {
                    branchLineShape.Vertices.Add(new Vertex(coord.X, coord.Y));
                }

                DHI.MHydro.Data.BranchPfs branch = new DHI.MHydro.Data.BranchPfs(branchLineShape, MHData);

                branch.UDCList.Add(new DHI.MHydro.Data.UserDefinedChainageVertexPfs(MHData, branch.ObjectID, 0, M11linkChannel.StartChainage));
                branch.UDCList.Add(new DHI.MHydro.Data.UserDefinedChainageVertexPfs(MHData, branch.ObjectID, M11linkChannel.GeoPoints.Count - 1, M11linkChannel.EndChainage()));


                branch.ChainageMgr.CommitAllChanges(null);

                branch.Name = M11linkChannel.Name;
                branch.TopoID = "From_ISIS";
                MHData.SimulationCcp.GridSpaceList.Add(new DHI.MHydro.Data.GridSpacePfs(branch.ObjectID, MHData.SimulationCcp.GridSpaceList.GlobalMaxDx, MHData));
                branch.FlowDirection = DHI.MHydro.Data.FlowDirection.Positive;

                branch.IsDirty = true;

                if (!nameGuidDict.ContainsKey(branch.Name))
                    nameGuidDict.Add(branch.Name, branch.ObjectID);
                else
                    System.Diagnostics.Debug.Assert(false, "Dumplicate type");


                LinkChannelPfs linkChannel = branch.LinkChannel;
                linkChannel.Type = LinkChannelType.DepthWidth;
                branch.BranchType = BranchType.LinkChannel;

                linkChannel.USInvert = M11linkChannel.BedLevelUs;
                linkChannel.DSInvert = M11linkChannel.BedLevelDs;
                if (M11linkChannel.DepthWidthCollection != null)
                {
                    for (int i = 0; i < M11linkChannel.DepthWidthCollection.Count; i++)
                    {
                        DepthWidth newData = new DepthWidth();
                        newData.Depth = M11linkChannel.DepthWidthCollection[i].Depth;
                        newData.Width = M11linkChannel.DepthWidthCollection[i].Width;
                        linkChannel.DepthWidthList.Add(newData);
                    }
                }

                MHData.BranchList.Add(branch);
                #endregion

                #region connection

                string connectionId = string.Empty;
                double connectChainage = 0.0f;
                if (!nameGuidDict.ContainsKey(M11linkChannel.Name)) continue;

                //TODO: UDC 
                //Connect if start connection is defined and if connection-to-branch is found
                if (GetBranchStartConnectionData(M11linkChannel, ref connectionId, ref connectChainage) && nameGuidDict.ContainsKey(connectionId))
                {
                    DHI.MHydro.Data.BranchPfs startBranch = MHData.BranchList[nameGuidDict[M11linkChannel.Name]];

                    DHI.MHydro.Data.BranchPfs connect2Branch = MHData.BranchList[nameGuidDict[connectionId]];

                    DHI.MHydro.Data.RiverNodePfs riverNode = connect2Branch.AddRiverNode(connectChainage);
                    startBranch.UpStreamConnRiverNodeID = riverNode.ObjectID;

                    LineShape line = new LineShape();
                    Vertex v = startBranch.Shape.Vertices[0];
                    line.Vertices.Add(new Vertex(v.X, v.Y));
                    line.Vertices.Add(new Vertex(riverNode.Shape.X, riverNode.Shape.Y));
                    DHI.MHydro.Data.ConnectionPfs conn = new DHI.MHydro.Data.ConnectionPfs(line, MHData);
                    conn.RiverNodeID = riverNode.ObjectID;
                    conn.StartBranchID = startBranch.ObjectID;
                    conn.StartPoint = StartingPoint.FirstPoint;
                    MHData.ConnectionList.Add(conn);

                }

                //TODO: UDC
                //Connect if end connection is defined and if connection-to-branch is found
                if (GetBranchEndConnectionData(M11linkChannel, ref connectionId, ref connectChainage) && nameGuidDict.ContainsKey(connectionId))
                {
                    DHI.MHydro.Data.BranchPfs startBranch = MHData.BranchList[nameGuidDict[M11linkChannel.Name]];
                    DHI.MHydro.Data.BranchPfs connect2Branch = MHData.BranchList[nameGuidDict[connectionId]];
                    DHI.MHydro.Data.RiverNodePfs riverNode = connect2Branch.AddRiverNode(connectChainage);

                    startBranch.DownStreamConnRiverNodeID = riverNode.ObjectID;

                    LineShape line = new LineShape();
                    Vertex v = startBranch.Shape.Vertices[startBranch.Shape.Vertices.Count - 1];
                    line.Vertices.Add(new Vertex(v.X, v.Y));
                    line.Vertices.Add(new Vertex(riverNode.Shape.X, riverNode.Shape.Y));
                    DHI.MHydro.Data.ConnectionPfs conn = new DHI.MHydro.Data.ConnectionPfs(line, MHData);
                    conn.RiverNodeID = riverNode.ObjectID;
                    conn.StartBranchID = startBranch.ObjectID;
                    conn.StartPoint = StartingPoint.LastPoint;
                    MHData.ConnectionList.Add(conn);
                }
                #endregion
            }
        }

        private void Import_NetWorkData(DHI.MHydro.Data.RootPfs MHData)
        {
            MHData.Simulation.Type = DHI.MHydro.Data.EnumSimulationType.MIKE11;

            #region coordinate systems
            DHI.Mike1D.Generic.Spatial.Geometry.Extent dataAreaExtend = new DHI.Mike1D.Generic.Spatial.Geometry.Extent();

            // set coordinate system undefined.
            MHData.CoordinateSystem.FeatureProjection = "NON-UTM";  //TODO: User property instead of hard coded 
            MHData.CoordinateSystem.MapProjection = "NON-UTM";
            MHData.BackgroundMap.MapType = DHI.MHydro.Data.EnumBackgroundMapType.None;
            MHData.BackgroundMap.MapVisible = false;
            #endregion

            #region Import branches

            foreach (RiverReachClass RiverReach in RiverReachCollection)
            {
                LineShape branchLineShape = new LineShape();
                foreach (GeoDigiPointClass coord in RiverReach.GeoPoints)
                {
                    branchLineShape.Vertices.Add(new Vertex(coord.X, coord.Y));

                    if (dataAreaExtend.XMin > coord.X)
                        dataAreaExtend.XMin = coord.X;
                    if (dataAreaExtend.XMax < coord.X)
                        dataAreaExtend.XMax = coord.X;
                    if (dataAreaExtend.YMin > coord.Y)
                        dataAreaExtend.YMin = coord.Y;
                    if (dataAreaExtend.YMax < coord.Y)
                        dataAreaExtend.YMax = coord.Y;
                }

                DHI.MHydro.Data.BranchPfs branch = new DHI.MHydro.Data.BranchPfs(branchLineShape, MHData);

                // Set user defined chaninage for first and last digipoint
                branch.UDCList.Add(new DHI.MHydro.Data.UserDefinedChainageVertexPfs(MHData, branch.ObjectID, 0, RiverReach.StartChainage));
                branch.UDCList.Add(new DHI.MHydro.Data.UserDefinedChainageVertexPfs(MHData, branch.ObjectID, RiverReach.GeoPoints.Count - 1, RiverReach.EndChainage()));

                branch.ChainageMgr.CommitAllChanges(null);

                branch.Name = RiverReach.Name;
                branch.TopoID = "From_ISIS";
                MHData.SimulationCcp.GridSpaceList.Add(new DHI.MHydro.Data.GridSpacePfs(branch.ObjectID, MHData.SimulationCcp.GridSpaceList.GlobalMaxDx, MHData));

                branch.IsDirty = true;

                if (!nameGuidDict.ContainsKey(branch.Name))
                    nameGuidDict.Add(branch.Name, branch.ObjectID);
                else
                    System.Diagnostics.Debug.Assert(false, "Dumplicate type");

                branch.FlowDirection = DHI.MHydro.Data.FlowDirection.Positive;
                branch.BranchType = DHI.MHydro.Data.BranchType.Regular;

                MHData.BranchList.Add(branch);
            } //End of foreach loop
            #endregion

            // Set working area extend
            if (dataAreaExtend != null)
            {
                PolygonShape csShape = new PolygonShape();
                csShape.OuterRing.Vertices.Add(new Vertex(dataAreaExtend.XMin, dataAreaExtend.YMax));
                csShape.OuterRing.Vertices.Add(new Vertex(dataAreaExtend.XMax, dataAreaExtend.YMax));
                csShape.OuterRing.Vertices.Add(new Vertex(dataAreaExtend.XMax, dataAreaExtend.YMin));
                csShape.OuterRing.Vertices.Add(new Vertex(dataAreaExtend.XMin, dataAreaExtend.YMin));
                csShape.OuterRing.Vertices.Add(new Vertex(dataAreaExtend.XMin, dataAreaExtend.YMax));
            }

            #region construction river node and connection.
            string connectionId = string.Empty;
            double connectChainage = 0.0f;
            foreach (RiverReachClass RiverReach in RiverReachCollection)
            {
                if (!nameGuidDict.ContainsKey(RiverReach.Name)) continue;

                //TODO: UDC 
                //Connect if start connection is defined and if connection-to-branch is found
                if (GetBranchStartConnectionData(RiverReach, ref connectionId, ref connectChainage) && nameGuidDict.ContainsKey(connectionId))
                {
                    DHI.MHydro.Data.BranchPfs startBranch = MHData.BranchList[nameGuidDict[RiverReach.Name]];

                    DHI.MHydro.Data.BranchPfs connect2Branch = MHData.BranchList[nameGuidDict[connectionId]];

                    DHI.MHydro.Data.RiverNodePfs riverNode = connect2Branch.AddRiverNode(connectChainage);
                    startBranch.UpStreamConnRiverNodeID = riverNode.ObjectID;

                    LineShape line = new LineShape();
                    Vertex v = startBranch.Shape.Vertices[0];
                    line.Vertices.Add(new Vertex(v.X, v.Y));
                    line.Vertices.Add(new Vertex(riverNode.Shape.X, riverNode.Shape.Y));
                    DHI.MHydro.Data.ConnectionPfs conn = new DHI.MHydro.Data.ConnectionPfs(line, MHData);
                    conn.RiverNodeID = riverNode.ObjectID;
                    conn.StartBranchID = startBranch.ObjectID;
                    conn.StartPoint = StartingPoint.FirstPoint;
                    MHData.ConnectionList.Add(conn);

                }

                //TODO: UDC
                //Connect if end connection is defined and if connection-to-branch is found
                if (GetBranchEndConnectionData(RiverReach, ref connectionId, ref connectChainage) && nameGuidDict.ContainsKey(connectionId))
                {
                    DHI.MHydro.Data.BranchPfs startBranch = MHData.BranchList[nameGuidDict[RiverReach.Name]];
                    DHI.MHydro.Data.BranchPfs connect2Branch = MHData.BranchList[nameGuidDict[connectionId]];
                    DHI.MHydro.Data.RiverNodePfs riverNode = connect2Branch.AddRiverNode(connectChainage);

                    startBranch.DownStreamConnRiverNodeID = riverNode.ObjectID;

                    LineShape line = new LineShape();
                    Vertex v = startBranch.Shape.Vertices[startBranch.Shape.Vertices.Count - 1];
                    line.Vertices.Add(new Vertex(v.X, v.Y));
                    line.Vertices.Add(new Vertex(riverNode.Shape.X, riverNode.Shape.Y));
                    DHI.MHydro.Data.ConnectionPfs conn = new DHI.MHydro.Data.ConnectionPfs(line, MHData);
                    conn.RiverNodeID = riverNode.ObjectID;
                    conn.StartBranchID = startBranch.ObjectID;
                    conn.StartPoint = StartingPoint.LastPoint;
                    MHData.ConnectionList.Add(conn);
                }

            } //End of foreach loop --- construction river node and connection
            #endregion
        }

        public bool GetBranchStartConnectionData(RiverReachClass reach, ref string connId, ref double connChainage)
        {
            if (reach == null || reach.UpStreamConnectionRiver == "")
                return false;

            try
            {
                connId = reach.UpStreamConnectionRiver;
                connChainage = reach.UpStreamConnectionChainage;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return false;
            }
        }

        public bool GetBranchEndConnectionData(RiverReachClass reach, ref string connId, ref double connChainage)
        {
            if (reach == null || reach.DownStreamConnectionRiver == "")
                return false;

            try
            {
                connId = reach.DownStreamConnectionRiver;
                connChainage = reach.DownStreamConnectionChainage;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return false;
            }
        }


        public void WriteLog(string logfile)
        {
            System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo();
            info.NumberDecimalSeparator = "."; info.NumberGroupSeparator = ",";
            info.NumberDecimalDigits = 3;



            System.IO.File.Delete(logfile);
            string logstring = "Number of river reaches: ";
            logstring = logstring + RiverReachCollection.Count.ToString() + System.Environment.NewLine;

            foreach (RiverReachClass Lreach in RiverReachCollection)
            {
                logstring = logstring + "       River: " + Lreach.Name + System.Environment.NewLine;
                foreach (SectionBaseClass LXsec in Lreach.XsecCollection)
                {
                    logstring = logstring + "           " + LXsec.Label[0] + System.Environment.NewLine;
                }
                logstring = logstring + "       Total number of sections: " + Lreach.XsecCollection.Count.ToString() + System.Environment.NewLine;
                logstring = logstring + "       Downstream connection: " + Lreach.DownStreamConnectionRiver + ", " + Lreach.DownStreamConnectionChainage.ToString() + System.Environment.NewLine;
                logstring = logstring + "       Upstream connection: " + Lreach.UpStreamConnectionRiver + ", " + Lreach.UpStreamConnectionChainage.ToString() + System.Environment.NewLine;
            }


            logstring = logstring + "Number of Abstraction sections: ";
            logstring = logstring + AbstractionCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Bernouli sections: ";
            logstring = logstring + BernoulliLossCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Blockage sections: ";
            logstring = logstring + BlockageCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of QH-boundary sections: ";
            logstring = logstring + QHBoundaryCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of H-boundary sections: ";
            logstring = logstring + HtBoundaryCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Q-boundary sections: ";
            logstring = logstring + QtBoundaryCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Tidal-boundary sections: ";
            logstring = logstring + TidalBoundaryCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Rainfall-Evaporation sections: ";
            logstring = logstring + RainEvapInfiltrationCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Normal/critical-boundary sections: ";
            logstring = logstring + NormalCriticalBoundaryCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Bridge sections: ";
            logstring = logstring + BridgeCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Conduit sections: ";
            logstring = logstring + ConduitCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Culvert sections: ";
            logstring = logstring + CulvertCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Flood plain sections: ";
            logstring = logstring + FloodPlainCollection.Count.ToString() + System.Environment.NewLine;
            // gauge
            logstring = logstring + "Number of Flood General head loss sections: ";
            logstring = logstring + HeadLossCollection.Count.ToString() + System.Environment.NewLine;
            // hydraulogical boundaries
            // Interpolate  handled under river
            logstring = logstring + "Number of Junction sections: ";
            logstring = logstring + JunctionCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Lateral sections: ";
            logstring = logstring + LateralCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of manhole sections: ";
            logstring = logstring + ManholeCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of orifice sections: ";
            logstring = logstring + OrificeCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of pond sections: ";
            logstring = logstring + PondCollection.Count.ToString() + System.Environment.NewLine;
            // Replicate  handled under river
            logstring = logstring + "Number of pump sections: ";
            logstring = logstring + PumpCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of rating curve sections: ";
            logstring = logstring + RatingCurveCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of reservoir sections: ";
            logstring = logstring + ReservoirCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Sluice sections: ";
            logstring = logstring + SluiceCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Spill sections: ";
            logstring = logstring + SpillCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Crump sections: ";
            logstring = logstring + CrumpCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Gated Weir sections: ";
            logstring = logstring + GatedWeirCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Notional Weir sections: ";
            logstring = logstring + NotionalWeirCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Flat V Weir sections: ";
            logstring = logstring + FlatVWeirCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of QH control sections: ";
            logstring = logstring + QHControlCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Round Nose Broad Crested Weir sections: ";
            logstring = logstring + RoundNoseBroadCrestedWeirCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Labyrinth Weir sections: ";
            logstring = logstring + LabyrinthWeirCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Sharp Crested Weir sections: ";
            logstring = logstring + SharpCrestedWeirCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Syphon sections: ";
            logstring = logstring + SyphonCollection.Count.ToString() + System.Environment.NewLine;
            logstring = logstring + "Number of Weir sections: ";
            logstring = logstring + WeirCollection.Count.ToString() + System.Environment.NewLine;
            logstring += "The following lines have reading error. It may because of the line string doesn't match the expected columns width." + System.Environment.NewLine;
            logstring += String.Join(",", errLineNums);
            System.IO.File.AppendAllText(logfile, logstring);

            if (SetStatus != null)
                SetStatus(100, "");
        }
    }

}
