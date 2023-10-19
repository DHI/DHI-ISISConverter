using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ISISConverterEngine
{
    public class AbstractionClass
    {
        public enum SwitchTypes { Time, Logical }
        public String Keyword = "ABSTRACTION";
        public string Comment = "";
        public LabelCollectionClass ID;
        public SwitchTypes Swicth = SwitchTypes.Time; 
        public string ConnectionLabel = "";
        private int NdataSets = 0;
        public double TLag = 0;
        public double TimeUnitInSeconds = 1;
        public string TimeExtension = "";
        public class AbstrationTemporalDataClass
        {
            public enum OperationModeTypes { AUTO, MANUAL }
            public double t = 0;
            public OperationModeTypes OperationMode = OperationModeTypes.AUTO;
            public double AbstractionAmount = 0.0;

        }
        public List<AbstrationTemporalDataClass> AbstractionTemporalData;
        public AbstractionClass(string[] StArray,ref int i, ref List<int> errLineList)
            {
            bool OK=true;    
            LineReaderClass l = new LineReaderClass();
                Comment = l.GetComment(Keyword, StArray[i]);
                i++;
                ID = new LabelCollectionClass(StArray[i]);
                i++;
                if (l.GetString(StArray[i], 1, ref  OK) == "TIME")
                {
                    Swicth = SwitchTypes.Time;
                }
                else
                {
                    Swicth = SwitchTypes.Logical;
                }
                 ConnectionLabel =  l.GetLabel(StArray[i], 2);
                 i++;
                 NdataSets = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
                 TLag = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                 TimeUnitInSeconds = l.GetTimeUnitinSec(StArray[i], 3);
                 TimeExtension = l.GetTimeExtensionMethod(StArray[i], 4, ref OK);
                 i++;
                 if (Swicth == SwitchTypes.Time)
                 {
                     AbstractionTemporalData = new List<AbstrationTemporalDataClass>();
                     for(int ii=i;ii<i+NdataSets;ii++)
                     {
                        try
                        {
                            AbstrationTemporalDataClass TemporalData = new AbstrationTemporalDataClass();
                            TemporalData.t = l.GetDouble(StArray[ii], 1, ii, ref OK, ref errLineList);
                            string lstring = l.GetString(StArray[ii], 2, ref OK);
                            if (lstring == "AUTO")
                            {
                                TemporalData.OperationMode = AbstrationTemporalDataClass.OperationModeTypes.AUTO;
                            }
                            else
                            {
                                TemporalData.OperationMode = AbstrationTemporalDataClass.OperationModeTypes.MANUAL;
                            }
                            TemporalData.AbstractionAmount = l.GetDouble(StArray[ii], 3, ii, ref OK, ref errLineList);
                            if (!OK)
                            {
                                TemporalData.AbstractionAmount = 0;
                            }

                            AbstractionTemporalData.Add(TemporalData);
                        }
                        catch(Exception e)
                        {
                            i = ii;
                            throw e;
                        }
                     }
                     i = i + NdataSets - 1; 
                 }
                     else
                     {
                     // RULES to BE HANDLED here see page 3 of 5 under ABSTRACTION in ISIS manual
                     }

                




            }
        
        

    }
}
