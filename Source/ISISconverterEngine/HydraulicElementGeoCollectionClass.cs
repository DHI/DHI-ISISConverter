using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class HydraulicElementGeoCollectionClass
    {
        public List<HydraulicElementsGeoClass> ElementList;

        public HydraulicElementsGeoClass LocateElement(string IdElement)
        {
            foreach (HydraulicElementsGeoClass element in ElementList)
            {
                if (element.IDPoint != null)
                {
                    if (element.IDPoint.CompareTo(IdElement) == 0)
                    {
                        return element;
                    }
                }

            }
            return null;

        }
        private void InsertConnection(string Id1, string Id2)
        {
            foreach (HydraulicElementsGeoClass element in ElementList)
            {
                if (element.IDPoint != null)
                {
                    if (element.IDPoint.CompareTo(Id1) == 0)
                    {
                        HydraulicElementsGeoClass ConnectingElement = LocateElement(Id2);
                        element.ConnectionList.Add(ConnectingElement);

                    }
                }
            }
        }
        public HydraulicElementGeoCollectionClass()
        {
            ElementList = new List<HydraulicElementsGeoClass>();
        }

        public HydraulicElementGeoCollectionClass(string[] filestringArray)
        {


            int MaxNoLines = filestringArray.Count();
            int i = 0;
            string substring = "";
            string[] Elementstringarray;
            ElementList = new List<HydraulicElementsGeoClass>();
            while (i < MaxNoLines)
            {
                substring = filestringArray[i];
                if (substring.Contains("["))
                    if (substring.Contains("_"))
                    {
                        Elementstringarray = new string[3] { filestringArray[i], filestringArray[i + 1], filestringArray[i + 2] };
                        HydraulicElementsGeoClass hydraulicelement = new HydraulicElementsGeoClass(Elementstringarray);
                        ElementList.Add(hydraulicelement);
                        i = i + 3;
                    }
                i++;

            }
            ElementList.Sort(HydraulicElementsGeoClass.CompareElements);
            i = 0;
            bool connectiosectionfound = false;
            while ((i < MaxNoLines) && !connectiosectionfound)
            {
                substring = filestringArray[i];
                if (substring.Contains("[Connections]"))
                {
                    connectiosectionfound = true;
                    i++;
                    int NoConnections = System.Convert.ToInt32(filestringArray[i].Substring(16));
                    i++;
                    for (int j = i; j < NoConnections + i; j++)
                    {
                        int indexEqualSign = filestringArray[j].IndexOf("=");
                        int indexComma = filestringArray[j].IndexOf(",");
                        string Idpoint1 = filestringArray[j].Substring(indexEqualSign + 1, indexComma - indexEqualSign - 1);
                        string Idpoint2 = filestringArray[j].Substring(indexComma + 1);
                        InsertConnection(Idpoint1, Idpoint2);
                    }
                    i = i + NoConnections;
                }
                i++;

            }
        }


    }
}
