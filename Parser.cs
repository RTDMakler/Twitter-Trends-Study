using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text.Json;
using GMap.NET.WindowsForms;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tweet_Trends
{
    internal class Parser
    {
        public void ParseLocMes(string line,UserInfo UsInfo)
        {
            try
            {
                UsInfo.FillTweet(GetXPos(line), GetYPos(line), GetMes(line));
            }
            catch
            {
                Console.WriteLine("Tweet Error");
            }
        }
        float GetXPos(string line) => Convert.ToSingle(line.Split(' ')[0].Replace("[", "").Replace(",", "")); 
        float GetYPos(string line) => Convert.ToSingle(((line.Split(' ')[1]).Split('\t')[0]).Replace("]", "").Trim()); 
        string GetMes(string line) => Convert.ToString(line.Split("\t")[3]).ToLower();
        public void CorrelateSentMes(List<string> message,Dictionary<string, float> sentiments, UserInfo UsIn)
        {

            var sv = new Stopwatch();
            sv.Start();
            Parallel.For(0, message.Count, i =>
            {
                foreach (string sentiment in sentiments.Keys)
                {

                    //int markAmount = Regex.Matches(message[i], sentiment).Count;
                    int markAmount;
                    if (sentiment.Length > 3)
                        markAmount = message[i].Split(sentiment + " ").Length + message[i].Split(" " + sentiment + " ").Length
                        + message[i].Split(" " + sentiment + "\n").Length - 3;
                    else markAmount = message[i].Split(" " + sentiment + " ").Length - 1;
                    if (markAmount > 0)
                    {
                        //Console.WriteLine(message[i] + " " + sentiment);
                        sentiments.TryGetValue(sentiment, out float markKey);
                        UsIn.marks[i] += markKey * markAmount;
                    }
                }
            });
            sv.Stop();
            Console.WriteLine(sv.Elapsed);
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
