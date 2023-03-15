using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text.Json;
using GMap.NET.WindowsForms;
using System.Configuration;

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
                //if (marks[i]!=0)
                //Console.WriteLine(marks[i]);
            }
            return marks;
        }
        private List<GMap.NET.PointLatLng> GPoints(List<double> points)
        {
            List<GMap.NET.PointLatLng> gPoints = new List<GMap.NET.PointLatLng>();
            for (int i=0;i<points.Count ; i+=2)
            {
                gPoints.Add(new GMap.NET.PointLatLng(points[i], points[i+1]));
            }
            return gPoints;
        }
        public void jsPars(List<State> states,string link = "states.json")
        {
            var fileText = new StreamReader(link).ReadToEnd();
            ReadOnlySpan<byte> readOnlySpan = Encoding.UTF8.GetBytes(fileText);
            var reader = new Utf8JsonReader(readOnlySpan);
            List<double> points = new List<double>();
            bool first=true;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        {
                            if(!first)
                            states[states.Count - 1].gMapPolygons.Add(new GMapPolygon(GPoints(points), states[states.Count - 1].stateName));
                            first = false;
                            states.Add(new State());
                            states[states.Count - 1].stateName = reader.GetString();
                            points = new List<double>();
                            break; 
                        }
                    case JsonTokenType.Number:
                        {
                            float floatValue = reader.GetSingle();
                            points.Add((double)floatValue);
                            break;
                        }
                }
            }
        }
    }
}
