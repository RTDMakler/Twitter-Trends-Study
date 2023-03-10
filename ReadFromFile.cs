using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweet_Trends
{
    internal class ReadFromFile
    {
        public Dictionary<string, float> GetSentiments(string link)
        {
            return ConvertCSVToArr(link);
        }
        Dictionary<string, float> ConvertCSVToArr(string link)
        {
            var dict = new Dictionary<string, float>();
            using (StreamReader reader = new StreamReader(link))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    dict.Add(parts[0].ToLower(), Convert.ToSingle(parts[1]));
                }
            }
            return dict;
        }
        public void GetInfo(string link,List<float> XPos,List<float> YPos,List<string> Message)
        {
            string[] lines = System.IO.File.ReadAllLines(link);
            new Parser().ParseLocMes(lines, XPos, YPos, Message);
        }
#fdgfasgdarwsgt
    }
}
