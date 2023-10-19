using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ISISConverterEngine
{
    class LineReaderClass
    {
        int substringLength = 10;
        public static int labelLength = 8;
        System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo();
        public LineReaderClass()
        {
            info.NumberDecimalSeparator = ".";
            info.NumberGroupSeparator = ",";

        }
        private string GetSubstring(string line, int i, List<int> lengthFormat = null)
        {
            string substring = "";
            if(lengthFormat == null || lengthFormat.Count < i)
            {
                int NtoRead = substringLength;
                if (line.Length > (i - 1) * substringLength)
                {
                    if (line.Length < i * substringLength)
                    {
                        NtoRead = line.Length - (i - 1) * substringLength;
                    }

                }
                else
                    return "";
                substring = line.Substring((i - 1) * substringLength, NtoRead);
            }
            else
            {
                int start = 0;
                for(int j = 0; j < i - 1; j++)
                {
                    start += lengthFormat[j];
                }
                if(start + lengthFormat[i - 1] > line.Length)
                {
                    substring = line.Substring(start);
                }
                else
                {
                    substring = line.Substring(start, lengthFormat[i - 1]);
                }
            }

            substring = substring.Trim();
            return substring;
        }
        
        public double GetDouble(string line, int i, int lineNum, ref bool OK, ref List<int> errList, List<int> lengthFormat = null)
        {
            OK = true;
            string substring = GetSubstring(line, i, lengthFormat);
            double d = -999.999;
            if (substring == "")
            {
                OK = false;
                return -999.999;
            }
            else if(!Double.TryParse(substring, out d))
            {
                if (!errList.Contains(lineNum + 1))
                    errList.Add(lineNum + 1);
                return -999.999;
            }
            else
            {
                return System.Convert.ToDouble(substring, info);
            }

        }

        public int GetInt(string line, int i, int lineNum, ref bool OK, ref List<int> errList)
        {
            OK = true;
            string substring = GetSubstring(line, i);
            int n = -999;
            if (substring == "")
            {
                OK = false;
                return -999;
            }
            else if(!int.TryParse(substring, out n))
            {
                if (!errList.Contains(lineNum + 1))
                    errList.Add(lineNum + 1);
                return -999;
            }
            else
            {
                return System.Convert.ToInt32(substring, info);
            }

        }

        public string GetString(string line, int i, ref bool OK)
        {
            OK = true;
            string substring = GetSubstring(line, i);
            if (substring == "")
            {
                OK = false;
                return "";
            }
            else
            {
                return substring;
            }

        }
        public string GetComment(string KEYWORD, string fullLine)
        {
            return fullLine.Substring(KEYWORD.Length);
        }

        public string GetLabel(string fullline, int i)
        {
            string Res = fullline;
            if (fullline.Length > i * labelLength - 1)
            {
                Res = fullline.Substring((i - 1) * labelLength, labelLength);
            }
            else
            {
                if (fullline.Length > (i - 1) * labelLength)
                {
                    Res = fullline.Substring((i - 1) * labelLength);
                }
                else
                    Res = "";
            }

            

            return Res.Trim();

        }
        public double GetTimeUnitinSec(string fullline, int i)
        {
            double t = 1;
            bool OK = true;
            string tstring = GetString(fullline, i, ref OK);
            if (OK) 
            {

                switch (tstring.ToUpper())
                {

                    case "SECONDS":
                        t = 1;
                        break;

                    case "MINUTES":
                        t = 60;
                        break;
                    case "HOURS":
                        t = 60 * 60;
                        break;
                    case "DAYS":
                        t = 24 * 60 * 60;
                        break;
                    case "WEEKS":
                        t = 60 * 60 * 24 * 7;
                        break;
                    case "FORTNIGHTS":
                        t = 60 * 60 * 24 * 7 * 2;
                        break;
                    case "MONTHS":
                        t = 60 * 60 * 24 * 30;
                        break;
                    case "LUNAR":
                        t = 60 * 60 * 24 * 29.53059;
                        break;
                    case "QUARTER":
                        t = 60 * 60 * 24 * 365.242199 * 0.25;
                        break;
                    case "YEARS":
                        t = 60 * 60 * 24 * 365.242199;
                        break;
                    case "DECADES":
                        t = 60 * 60 * 24 * 365.242199 * 10;
                        break;

                    default:
                        t = 1;
                        break;
                }
            }
                else
                t = 1;
                 return t;
        }

        public string GetTimeExtensionMethod(string fullline,int i, ref bool OK)
        {
            string tstring = GetString(fullline, i, ref OK);
            if (tstring == "") tstring = "NOEXTEND";
            return tstring;
        }

        public int GetHour(string fullline, int i, ref bool OK)
        {
            int hour = 0;
            string tstring = GetSubstring(fullline, i);
            if (!(tstring == ""))
            {
                tstring = tstring.Substring(5, 2);
                hour = System.Convert.ToInt32(tstring);
            }
            return hour;
        }
        public int GetMinutes(string fullline, int i, ref bool OK)
        {
            int Min = 0;
            string tstring = GetSubstring(fullline, i);
            if (!(tstring == ""))
            {
                tstring = tstring.Substring(8, 2);
                Min = System.Convert.ToInt32(tstring);
            }
            return Min;
        }

        public int GetDay(string fullline, int i, ref bool OK)
        {
            int Day = 0;
            string tstring = GetSubstring(fullline, i);
            if (!(tstring == ""))
            {
                tstring = tstring.Substring(0, 2);
                Day = System.Convert.ToInt32(tstring);
            }
            return Day;
        }

        public int GetMonth(string fullline, int i, ref bool OK)
        {
            int Month = 0;
            string tstring = GetSubstring(fullline, i);
            if (!(tstring == ""))
            {
                tstring = tstring.Substring(3, 2);
                Month = System.Convert.ToInt32(tstring);
            }
            return Month;
        }

        public int GetYear(string fullline, int i, ref bool OK)
        {
            int Year = 1900;
            string tstring = GetSubstring(fullline, i);
            if (!(tstring == ""))
            {
                tstring = tstring.Substring(6, 4);
                Year = System.Convert.ToInt32(tstring);
            }
            return Year;
        }
    }
}
