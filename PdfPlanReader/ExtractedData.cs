using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfPlanReader
{
    internal class ExtractedData
    {
        public string Reg { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ALTN1 { get; set; }
        public string ALTN2 { get; set; }
        public string FltNr { get; set; }
        public string ATC { get; set; }
        public string ToFuelTime { get; set; }
        public decimal ToFuelValue { get; set; }
        public string AltFuelTime { get; set; }
        public decimal AltFuelValue { get; set; }
        public string STD { get; set; }
        public string STA { get; set; }
        public decimal MIN { get; set; }
        public int ZFM { get; set; }
        public int PAX { get; set; }
        public string GainLoss { get; set; }

        public static ExtractedData ReadFromPage(string page)
        {
            var ed = new ExtractedData();

            var tokens = page.Split(new char[0],StringSplitOptions.RemoveEmptyEntries);

            int i = 0;
            bool isGoodPage = false;
            while(i< tokens.Length)
            {
                switch(tokens[i].ToUpper()) 
                {
                    case "PAGE":
                        if (tokens[++i] == "1")
                        {
                            isGoodPage = true;
                        }
                        else if (tokens[++i] != "1" && !isGoodPage)
                            return null;
                        break;
                    case "REG.:":
                        ed.Reg = tokens[++i];
                        break;
                    case "FROM:":
                        ed.From = tokens[++i];
                        break;
                    case "TO:":
                        ed.To = tokens[++i];
                        break;
                    case "ALTN1:":
                        if(string.IsNullOrEmpty(ed.ALTN1))
                            ed.ALTN1 = tokens[++i];
                        break;
                    case "FLTNR:":
                        ed.FltNr = tokens[++i];
                        break;
                    case "ATC:":
                        ed.ATC = tokens[++i];
                        break;
                    case "STD:":
                        ed.STD = tokens[++i];
                        break;
                    case "STA:":
                        ed.STA = tokens[++i];
                        break;
                    case "MIN:":
                        i += 2;
                        ed.MIN = decimal.Parse(tokens[i], CultureInfo.InvariantCulture);
                        break;
                    case "ZFM:":
                        ed.ZFM = int.Parse(tokens[++i]);
                        break;
                    case "PAX:":
                        i += 3;
                        while (tokens[i].Length <= 1)
                        {
                            i++;
                        }
                        ed.PAX = int.Parse(Regex.Match(tokens[i], @"\d+").Value, NumberFormatInfo.InvariantInfo);
                        break;
                    case "GAIN":
                        if (tokens[++i]=="/" && tokens[++i].ToUpper()=="LOSS:")
                        {
                            ed.GainLoss = tokens[++i] + " " + tokens[++i];
                        }
                        break;
                    default:
                        if(tokens[i].ToUpper()==ed.To+":")
                        {
                            ed.ToFuelTime = tokens[++i];
                            ed.ToFuelValue = decimal.Parse(tokens[++i], CultureInfo.InvariantCulture);
                        }
                        else if (tokens[i].ToUpper()==ed.ALTN1+":")
                        {
                            ed.AltFuelTime = tokens[++i];
                            ed.AltFuelValue = decimal.Parse(tokens[++i], CultureInfo.InvariantCulture);
                        }
                        break;
                }
                i++;
            }

            return ed;
        }
    }
}
