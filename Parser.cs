using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Tweet_Trends
{
    internal class Parser
    {
        public void ParseLocMes(string[] lines,List<float> XPos, List<float> YPos, List<string> Message)
        {
            for(int i=0;i<lines.Length;i++) 
            {
                var curString = lines[i];
                XPos.Add(GetXPos(curString));
                YPos.Add(GetYPos(curString));
                Message.Add(GetMes(curString));

            }
        }
        float GetXPos(string line) => Convert.ToSingle(line.Split(' ')[0].Replace("[", "").Replace(",", "")); 
        float GetYPos(string line) => Convert.ToSingle(((line.Split(' ')[1]).Split('\t')[0]).Replace("]", "").Trim()); 
        string GetMes(string line) => Convert.ToString(line.Split("\t")[3]);
        public float[] CorrelateSentMes(List<string> message,Dictionary<string, float> sentiments)
        {
            float[] marks = new float[message.Count];
            for(int i=0;i<message.Count ; i++) 
            {
                foreach (string sentiment in sentiments.Keys)
                {

                    int markAmount = Regex.Matches(message[i], sentiment).Count;
                    if (markAmount != 0)
                    {
                        sentiments.TryGetValue(sentiment, out float markKey);
                        marks[i] += markKey * markAmount;
                    }
                }
                if (marks[i]!=0)
                Console.WriteLine(marks[i]);
            }
            return marks;
        }
    }
}
