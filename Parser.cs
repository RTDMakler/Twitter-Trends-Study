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
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace GMap
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
        float GetMark(string[] mesArr,Dictionary <string, float> sentiments)
        {
            float mark = 0;
                for (int i = 0; i < mesArr.Length; i++)
                {
                    mark += sentiments.GetValueOrDefault(mesArr[i]);

                }
                if (mesArr.Length >= 2)
                    for (int i = 0; i < mesArr.Length - 1; i++)
                    {
                        mark += sentiments.GetValueOrDefault(mesArr[i] + mesArr[i + 1]);
                    }
                if (mesArr.Length >= 3)
                    for (int i = 0; i < mesArr.Length - 2; i++)
                    {
                        mark += sentiments.GetValueOrDefault(mesArr[i] + mesArr[i + 1] + mesArr[i + 2]);
                    }
                if (mesArr.Length >= 4)
                    for (int i = 0; i < mesArr.Length - 3; i++)
                    {
                        mark += sentiments.GetValueOrDefault(mesArr[i] + mesArr[i + 1] + mesArr[i + 2] + mesArr[i + 3]);
                    }
            return (float)mark;
        }
        public void CorrelateSentMes(List<string> message,Dictionary<string, float> sentiments, UserInfo UsIn)
        {

            Parallel.For(0, message.Count, i =>
            {
                var mesArr = message[i].Split(" ");
                UsIn.marks[i] += GetMark(mesArr, sentiments);
            });
        }
        private List<GMap.NET.PointLatLng> GPoints(List<double> points)
        {
            List<GMap.NET.PointLatLng> gPoints = new List<GMap.NET.PointLatLng>();
            for (int i=0;i<points.Count ; i+=2)
            {
                gPoints.Add(new GMap.NET.PointLatLng(points[i+1], points[i]));
            }
            return gPoints;
        }
        public void jsPars(List<State> states, string link = "states.json")
        {
            var fileText = new StreamReader(link).ReadToEnd();
            ReadOnlySpan<byte> readOnlySpan = Encoding.UTF8.GetBytes(fileText);
            var reader = new Utf8JsonReader(readOnlySpan);
            List<double> points = new List<double>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        {
                            if(points.Count != 0)
                            states[states.Count - 1].gMapPolygons.Add(new GMapPolygon(GPoints(points), states[states.Count - 1].stateName));
                            states.Add(new State());
                            states[states.Count-1].stateName = reader.GetString();
                            points = new List<double>();
                            break;
                        }
                    case JsonTokenType.Number:
                        {
                            float value = reader.GetSingle();
                            points.Add((double)value);
                            if (points.Count>=5 && points.Count%2==0)
                            {
                                if (points[points.Count-1] == points[1] && points[points.Count - 2] == points[0])
                                {
                                    states[states.Count - 1].gMapPolygons.Add(new GMapPolygon(GPoints(points), states[states.Count - 1].stateName));
                                    points= new List<double>();
                                }
                            }
                            break;
                        }
                }
            }
        }
    }
}
