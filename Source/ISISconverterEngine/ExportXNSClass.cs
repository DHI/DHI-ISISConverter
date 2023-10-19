using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DHI.Mike1D.CrossSectionModule;
using DHI.Mike1D.Generic;
using DHI.Mike1D.Generic.Spatial.Geometry;

namespace ISISConverterEngine
{
    public class ExportXNSClass
    {
        CrossSectionData XNS1 = new CrossSectionData();

        public void Save(string fileName)
        {
            XNS1.Connection = Connection.Create(fileName);
 
                CrossSectionDataFactory.Save(XNS1);

        }



        public void CreateReachCrossSections(RiverReachClass IsisReach)
        {

            string RiverName = IsisReach.Name;
            int NumberInReach = IsisReach.XsecCollection.Count;
            double chainage = 0;
            double LeftX = 0, LeftY = 0, RightX = 0, RightY = 0;
            for (int i = 0; (i < NumberInReach); i++)
            {
                SectionBaseClass IsisXS = IsisReach.XsecCollection[i];
                CrossSectionFactory factory = new CrossSectionFactory();
                CrossSectionLocated xsLocated;
                string XSID = "copy";
                if (IsisXS.Label.Count >0) XSID = IsisXS.Label[0];


                var location = new ZLocation(RiverName, chainage);
                chainage = chainage + IsisXS.deltaChainage;
                if (!(IsisXS.Keyword == "INTERPOLATE"))
                {
                    if (IsisXS is CrossSectionClass)
                    {
                        factory.BuildOpen(XSID);
                        factory.SetLocation(location);
                        factory.SetResistanceDistribution(ResistanceDistribution.Distributed);

                        FlowResistance flowResistance = new FlowResistance();
                        flowResistance.Formulation = ResistanceFormulation.Manning_n;
                        flowResistance.ResistanceDistribution = ResistanceDistribution.Distributed;
                        factory.SetFlowResistance(flowResistance);


                        factory.SetRadiusType(RadiusType.HydraulicRadiusTotalArea);
                        if (IsisXS.NumberOfPoints > 0)
                        {
                            CrossSectionPointList crsPoints = new CrossSectionPointList();
                            CrossSectionClass IsisXS2 = IsisXS as CrossSectionClass;
                            for (int ii = 0; (ii < IsisXS.NumberOfPoints); ii++)
                            {

                                double X = IsisXS2.Surveydata[ii].x;
                                double Z = IsisXS2.Surveydata[ii].z;
                                double n = IsisXS2.Surveydata[ii].n;
                                CrossSectionPoint Xsecp = new CrossSectionPoint(X, Z) { DistributedResistance = n };

                                crsPoints.Add(Xsecp);

                            }


                            factory.SetRawPoints(crsPoints);
                            if (IsisXS2.marker1 > -1)
                            {
                                factory.SetLeftLeveeBank(crsPoints[IsisXS2.marker1]);


                            }
                            else
                            {
                                factory.SetLeftLeveeBank(crsPoints[0]);
                            }
                            factory.SetLowestPoint(crsPoints[IsisXS2.marker2]);
                            factory.SetRightLeveeBank(crsPoints[IsisXS2.marker3]);



                            if (IsisXS2.marker1 > -1)
                            {
                                LeftX = IsisXS2.Surveydata[IsisXS2.marker1].GeoX;
                                LeftY = IsisXS2.Surveydata[IsisXS2.marker1].GeoY;
                            }
                            else
                            {
                                LeftX = 0; LeftY = 0;
                            }
                            if (IsisXS2.marker3 > -1)
                            {
                                RightX = IsisXS2.Surveydata[IsisXS2.marker3].GeoX;
                                RightY = IsisXS2.Surveydata[IsisXS2.marker3].GeoY;
                            }
                            else
                            {
                                RightX = 0; RightY = 0;
                            }
                        }
                    }
                    if (IsisXS is ConduitClass)
                    {
                        ConduitClass ClosedXs = (IsisXS as ConduitClass);
                        string Keyword2 = ClosedXs.Keyword2;
                        switch (Keyword2)
                        {
                            case "CIRCULAR":
                                {
                                    factory.BuildCircular(ClosedXs.diameter);
                                    location.Z = ClosedXs.invert;
                                    factory.SetLocation(location);
                                    FlowResistance flowResistance = new FlowResistance();
                                    flowResistance.Formulation = ResistanceFormulation.Manning_n;
                                    flowResistance.ResistanceDistribution = ResistanceDistribution.Uniform;
                                    if (ClosedXs.Fric == ConduitClass.FricTypes.Manningsn)
                                    {
                                        flowResistance.ResistanceValue = ClosedXs.fricnumber1;
                                    }
                                    else
                                    {
                                        flowResistance.ResistanceValue = 25.4 / Math.Pow(ClosedXs.fricnumber1, 1 / 6.0);
                                    }
                                    factory.SetFlowResistance(flowResistance);
                                    xsLocated = factory.GetCrossSection();
                                    xsLocated.Info = XSID;
                                    break;
                                }
                            case "FULLARCH":
                                {
                                    ClosedXs.height2 = ClosedXs.height;
                                    ClosedXs.height = 0;
                                    goto case "SPRUNGARCH";

                                }
                            case "SECTION": // symmetrical
                                {
                                    factory.BuildPolygon(XSID);
                                    location.Z = ClosedXs.invert;
                                    factory.SetLocation(location);
                                    factory.SetResistanceDistribution(ResistanceDistribution.Uniform);
                                    FlowResistance flowResistance = new FlowResistance();
                                    flowResistance.Formulation = ResistanceFormulation.Manning_n;
                                    factory.SetRadiusType(RadiusType.HydraulicRadiusTotalArea);
                                    flowResistance.ResistanceDistribution = ResistanceDistribution.Uniform;
                                    CrossSectionPointList crsPoints = new CrossSectionPointList();

                                    for (int ii = ClosedXs.NumberOfPoints - 1; (ii > -1); ii--)
                                    {
                                        double X = -ClosedXs.DataPointCollection[ii].x;

                                        double Z = ClosedXs.DataPointCollection[ii].y;


                                        CrossSectionPoint Xsecp = new CrossSectionPoint(X, Z);

                                        crsPoints.Add(Xsecp);

                                    }
                                    double ktotal = 0;
                                    double ltotal = 0;
                                    for (int ii = 1; (ii < ClosedXs.NumberOfPoints); ii++)
                                    {
                                        double X = ClosedXs.DataPointCollection[ii].x;

                                        double Z = ClosedXs.DataPointCollection[ii].y;
                                        double delXsqr = (X - ClosedXs.DataPointCollection[ii - 1].x) * (X - ClosedXs.DataPointCollection[ii - 1].x);
                                        double delYsqr = (Z - ClosedXs.DataPointCollection[ii - 1].y) * (Z - ClosedXs.DataPointCollection[ii - 1].y);
                                        double length = Math.Sqrt(delXsqr + delYsqr);
                                        ktotal = ClosedXs.DataPointCollection[ii].k * length;
                                        ltotal = ltotal + length;
                                        CrossSectionPoint Xsecp = new CrossSectionPoint(X, Z);

                                        crsPoints.Add(Xsecp);

                                    }
                                    ClosedXs.fricnumber1 = ktotal / ltotal;



                                    factory.SetRawPoints(crsPoints);
                                    if (ClosedXs.Fric == ConduitClass.FricTypes.Manningsn)
                                    {
                                        flowResistance.ResistanceValue = ClosedXs.fricnumber1;
                                    }
                                    else
                                    {
                                        flowResistance.ResistanceValue = 25.4 / Math.Pow(ClosedXs.fricnumber1, 1 / 6.0);
                                    }
                                    factory.SetFlowResistance(flowResistance);
                                    break;
                                }
                            case "RECTANGULAR":
                                {
                                    factory.BuildRectangular(ClosedXs.height, ClosedXs.width);
                                    location.Z = ClosedXs.invert;
                                    factory.SetLocation(location);
                                    FlowResistance flowResistance = new FlowResistance();
                                    flowResistance.Formulation = ResistanceFormulation.Manning_n;
                                    flowResistance.ResistanceDistribution = ResistanceDistribution.Uniform;
                                    if (ClosedXs.Fric == ConduitClass.FricTypes.Manningsn)
                                    {
                                        flowResistance.ResistanceValue = ClosedXs.fricnumber1;
                                    }
                                    else
                                    {
                                        flowResistance.ResistanceValue = 25.4 / Math.Pow(ClosedXs.fricnumber1, 1 / 6.0);
                                    }
                                    factory.SetFlowResistance(flowResistance);
                                    xsLocated = factory.GetCrossSection();
                                    xsLocated.Info = XSID;
                                    break;
                                }
                            case "SPRUNG": goto case "SPRUNGARCH";
                            case "SPRUNGARCH":
                                {
                                    factory.BuildPolygon(XSID);
                                    location.Z = ClosedXs.invert;
                                    factory.SetLocation(location);
                                    factory.SetRadiusType(RadiusType.HydraulicRadiusTotalArea);
                                    factory.SetResistanceDistribution(ResistanceDistribution.Uniform);
                                    FlowResistance flowResistance = new FlowResistance();
                                    flowResistance.Formulation = ResistanceFormulation.Manning_n;
                                    flowResistance.ResistanceDistribution = ResistanceDistribution.Uniform;
                                    if (ClosedXs.Fric == ConduitClass.FricTypes.Manningsn)
                                    {
                                        flowResistance.ResistanceValue = ClosedXs.fricnumber1;
                                    }
                                    else
                                    {
                                        flowResistance.ResistanceValue = 25.4 / Math.Pow(ClosedXs.fricnumber1, 1 / 6.0);
                                    }
                                    factory.SetFlowResistance(flowResistance);

                                    CrossSectionPointList crsPoints = new CrossSectionPointList();
                                    ClosedXs.height2 = Math.Min(ClosedXs.height2, ClosedXs.width / 2); // sanity check on values
                                    int nxlevels = 21;
                                    double delx = ClosedXs.width / 2 / (nxlevels - 1);
                                    double Zc = ClosedXs.height2 / 2 + ClosedXs.height - ClosedXs.width * ClosedXs.width / 8 / ClosedXs.height2;
                                    double Radius = ClosedXs.height+ClosedXs.height2 - Zc;
                                    for (int ii = 0; (ii < nxlevels - 1); ii++)
                                    {
                                        double X = ii * delx;
                                        double Z = Zc + Math.Sqrt(Math.Max(Radius * Radius - X * X, 0));
                                        CrossSectionPoint Xsecp = new CrossSectionPoint(X, Z);

                                        crsPoints.Add(Xsecp);

                                    }
                                    CrossSectionPoint Xsecp2 = new CrossSectionPoint(0.5 * ClosedXs.width, ClosedXs.height);
                                    crsPoints.Add(Xsecp2);
                                    Xsecp2 = new CrossSectionPoint(0.5 * ClosedXs.width, 0);
                                    crsPoints.Add(Xsecp2);
                                    Xsecp2 = new CrossSectionPoint(-0.5 * ClosedXs.width, 0);
                                    crsPoints.Add(Xsecp2);
                                    Xsecp2 = new CrossSectionPoint(-0.5 * ClosedXs.width, ClosedXs.height);
                                    crsPoints.Add(Xsecp2);
                                    for (int ii = 1; (ii < nxlevels - 1); ii++)
                                    {
                                        double X = -0.5 * ClosedXs.width + ii * delx;
                                        double Z = Zc + Math.Sqrt(Math.Max(Radius * Radius - X * X, 0));
                                        CrossSectionPoint Xsecp = new CrossSectionPoint(X, Z);

                                        crsPoints.Add(Xsecp);

                                    }
                                    factory.SetRawPoints(crsPoints);
                                    break;
                                }
                        }
                    }
                    xsLocated = factory.GetCrossSection();

                    // Calculates the processed levels, storage areas, radii, etc, ie, fill in all 
                    xsLocated.TopoID = "From_ISIS";
                    if (Math.Abs(RightX - LeftX) + Math.Abs(RightY - LeftY) > 0.01)
                    {
                        xsLocated.ApplyCoordinates = true;
                        xsLocated.Coordinates = CoordinateList.Create(CoordinateType.XY, 2);
                        xsLocated.Coordinates.Add(new Coordinate() { X = LeftX, Y = LeftY });
                        xsLocated.Coordinates.Add(new Coordinate() { X = RightX, Y = RightY });
                    }

                    XNS1.Add(xsLocated);
                }
            }
        }
    }
}



