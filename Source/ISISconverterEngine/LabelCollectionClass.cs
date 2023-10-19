using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class LabelCollectionClass
    {
        public List<string> Labels;
        public LabelCollectionClass(string stringlabel)
        {
            Labels = new List<string>();
            string stest = "NOTAssigned";
            int ii = 1;
            LineReaderClass l = new LineReaderClass();
            while (!(stest == ""))
            {
                stest = l.GetLabel(stringlabel, ii);
                if (!(stest == ""))
                {
                    Labels.Add(stest);
                }
                ii++;
            }
        }
        public LabelCollectionClass()
        {
            Labels = new List<string>();
        
        }
    }
}
