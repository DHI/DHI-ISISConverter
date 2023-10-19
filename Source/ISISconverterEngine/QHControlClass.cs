using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISISConverterEngine
{
    public class QHpairclass
    {
        public double Q, H;
    }

    public class QHControlClass : StructureClass
    {
        public double zc, m;
        public int NQHpairs;
        public QHpairclass[] QHData;

        public QHControlClass(string[] StArray, ref int i, ref List<int> errLineList)
        {
            Keyword = "QH CONTROL";
            bool OK = true;
            LineReaderClass l = new LineReaderClass();
            Comment = l.GetComment(Keyword, StArray[i]);
            i++;
            OK = true;
            ID = new LabelCollectionClass(StArray[i]);
            i++;
            zc = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
            m = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
            i++;
            NQHpairs = l.GetInt(StArray[i], 1, i, ref OK, ref errLineList);
            QHData = new QHpairclass[NQHpairs];
            for (int iii = 0; iii < NQHpairs; iii++)
            {
                i++;
                QHpairclass lQH = new QHpairclass();
                lQH.Q = l.GetDouble(StArray[i], 1, i, ref OK, ref errLineList);
                lQH.H = l.GetDouble(StArray[i], 2, i, ref OK, ref errLineList);
                QHData[iii] = lQH;
            }
         
        }

        double drownf(double Hup, double Hds)
        {
            double df = 1;
            double d1 = Math.Max(Hup - zc,0);
            double d2 = Math.Max(Hds - zc, 0);
            if (d1 > d2)
            {
                double dummy = d2;
                d2 = d1;
                d1 = dummy;
            }
            if (d1 > 0)
            if (d2 / d1 >= m)
            {
                df = Math.Sqrt((1 - d2 / d1) / (1 - m));
                if (df < 0.3)
                    df = (1 - d2 / d1) / (0.3 * (1 - m));
            }
 
            return df;
        }

        public override MIKE11StructureClass CreateMIKE11Structure(StructureClass lstructure)
        {
            MIKE11TabulatedStructureClass M11TabulatedStructure = new MIKE11TabulatedStructureClass(lstructure);
            M11TabulatedStructure.Chainage = Chainage;
            M11TabulatedStructure.RiverName = RiverName;
            M11TabulatedStructure.ID = Keyword + " " + ID.Labels[0] + " " + Comment;
            M11TabulatedStructure.TabulatedType = MIKE11TabulatedStructureClass.TabulatedTypes.QHusHds;
            M11TabulatedStructure.Hup = new double[NQHpairs + 2]; //add 2 points below crest
            M11TabulatedStructure.Hdown = new double[NQHpairs + 2]; //add 2 points below crest
            M11TabulatedStructure.Q =     new double[NQHpairs + 2, NQHpairs + 2];
            // set values below crest
            if (QHData[0].H > zc)
            {
                M11TabulatedStructure.Hup[0] = zc - 5;
                M11TabulatedStructure.Hdown[0] = zc - 5;
                M11TabulatedStructure.Hup[1] = zc;
                M11TabulatedStructure.Hdown[1] = zc;
            }
            else
            {
                M11TabulatedStructure.Hup[0] = QHData[0].H - 5;
                M11TabulatedStructure.Hdown[0] = QHData[0].H - 5;
                M11TabulatedStructure.Hup[1] = zc - 0.1;
                M11TabulatedStructure.Hdown[1] = zc - 0.1;
            }
            
            // transfer water level values
            for (int iii = 0; iii < NQHpairs; iii++)
            {
                M11TabulatedStructure.Hup[iii+2] = QHData[iii].H;
                M11TabulatedStructure.Hdown[iii + 2] = QHData[iii].H;
            }

            for (int iii = 0; iii < NQHpairs+2; iii++) // populate based on QH curve and droning coefficeint 
            {
                for (int iiii = 0; iiii < NQHpairs+2; iiii++)
                {
                    double Hvalue = M11TabulatedStructure.Hup[iii];
                    int sign = 1;
                    int index = iii-2;
                    if (M11TabulatedStructure.Hup[iii] < M11TabulatedStructure.Hdown[iiii])
                    {
                        Hvalue = M11TabulatedStructure.Hdown[iiii];
                        sign = -1;
                        index = iiii-2;
                    }
                    if (index > -1)
                        M11TabulatedStructure.Q[iii, iiii] = sign * drownf(M11TabulatedStructure.Hup[iii], M11TabulatedStructure.Hdown[iiii]) * QHData[index].Q;
                    else
                        M11TabulatedStructure.Q[iii, iiii] = 0;
                }
            }

            return M11TabulatedStructure;
        }
    }
}
