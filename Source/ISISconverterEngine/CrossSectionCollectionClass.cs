using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ISISConverterEngine
{
    public class CrossSectionCollectionClass
    {


        public List<SectionBaseClass> XsecList;
        public CrossSectionCollectionClass()
        {
            XsecList = new List<SectionBaseClass>();
            
        }
        
       
        private string createHeader(string rivername, string topoID, double chainage, string comment)
        {
            string header= topoID + System.Environment.NewLine;
            header = header + rivername + System.Environment.NewLine;
            header = header +chainage.ToString()+System.Environment.NewLine;
            header = header +"COORDINATES\n0\nFLOW DIRECTION\n0\nPROTECT DATA\n0\nDATUM\n0.00\nRADIUS TYPE\n0\n";
            header = header + "DIVIDE X-Section\n0\nSECTION ID\n"+comment+"\nINTERPOLATED\n0\nANGLE\n0.00   0\nRESISTANCE NUMBERS\n   2  1     1.000     1.000     1.000    1.000    1.000 \n";
            return header;
        }

        private string FormatSubstring(string astring)
        {
            int dotpos = astring.IndexOf(".");
            if (dotpos < 0)
            {
                astring = astring + ".000";

            }
            else
            {
                int Npad = 4 - astring.Length + dotpos;
                if (dotpos > 0)
                    astring = astring.PadRight(astring.Length + Npad, '0');
            }
            return astring.PadLeft(10);
        }

        public void exportToTxtFile(string exportfile)
        {
            
            System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo(); 
            info.NumberDecimalSeparator = "."; info.NumberGroupSeparator = ",";
            info.NumberDecimalDigits =3;
            
            
            
            System.IO.File.Delete(exportfile);
            foreach (CrossSectionClass xsec in XsecList)
            {
                string XSecstring = createHeader("River", "Topo?", 0, xsec.Comment);
                XSecstring = XSecstring + "PROFILE        " + xsec.NumberOfPoints.ToString() + System.Environment.NewLine;
                for (int i = 0; i < xsec.NumberOfPoints; i++)
                {
                    
                    
                   

                    XSecstring = XSecstring + FormatSubstring(xsec.Surveydata[i].x.ToString(info));
                    XSecstring = XSecstring + FormatSubstring(xsec.Surveydata[i].z.ToString(info));
                    XSecstring = XSecstring + FormatSubstring(xsec.Surveydata[i].n.ToString(info));
                    
                    int marker = 0;
                    if (xsec.marker1 == i)
                    {
                        marker = marker + 1;
                    }
                    if (xsec.marker2 == i)
                    {
                        marker = marker + 2;
                    }
                    if (xsec.marker3 == i)
                    {
                        marker = marker + 4;
                    }
                    string substring = "<#" + marker.ToString() + "> ";
                    XSecstring = XSecstring + substring.PadLeft(10);
                    XSecstring = XSecstring + "    0     0.000     0\n";
                    




                }
                XSecstring = XSecstring + "*******************************\n";
                System.IO.File.AppendAllText(exportfile, XSecstring);

               
            }
            

        }

    }

}

