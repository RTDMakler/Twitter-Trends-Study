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
        float f1(string[] mesArr, Dictionary<string, float> sentiments, string sentiment)
        {
            float mark=0;
            for(int i=0;i<mesArr.Length;i++) 
            {
                if (sentiment == mesArr[i])
                    mark += sentiments.GetValueOrDefault(mesArr[i]);
            }
            return mark;
        }
        float f2(string[] mesArr, Dictionary<string, float> sentiments, string sentiment) 
        {
            float mark = 0;
            for (int i = 0; i < mesArr.Length; i++)
                for (int j = 0; j < mesArr.Length; j++)
                {
                    if (sentiment == (mesArr[i]+" "+ mesArr[j]))
                        mark += sentiments.GetValueOrDefault(mesArr[i] + " " + mesArr[j]);
                }
            return mark;
        }
        float f3(string[] mesArr, Dictionary<string, float> sentiments, string sentiment)
        {
            float mark = 0;
            for (int i = 0; i < mesArr.Length; i++)
                for (int j = 0; j < mesArr.Length; j++)
                    for (int k = 0; k < mesArr.Length; k++)
                    {
                    if (sentiment == (mesArr[i] + " " + mesArr[j]+ " " + mesArr[k]))
                        mark += sentiments.GetValueOrDefault(mesArr[i] + " " + mesArr[j] + " " + mesArr[k]);
                }
            return mark;
        }
        float f4(string[] mesArr, Dictionary<string, float> sentiments, string sentiment)
        {
            float mark = 0;
            for (int i = 0; i < mesArr.Length; i++)
                for (int j = 0; j < mesArr.Length; j++)
                    for (int k = 0; k < mesArr.Length; k++)
                        for (int g = 0; g < mesArr.Length; g++)
                        {
                        if (sentiment == (mesArr[i] + " " + mesArr[j] + " " + mesArr[k] +" "+ mesArr[j]))
                            mark += sentiments.GetValueOrDefault(mesArr[i] + " " + mesArr[j] + " " + mesArr[k] + " " + mesArr[j]);
                    }
            return mark;
        }
        float GetMark(string[] mesArr,Dictionary <string, float> sentiments)
        {
            int size;
            float mark = 0;
            foreach (var sentiment in sentiments.Keys)
            {
                size = sentiment.Split(" ").Length;
             
                switch (size)
                {
                    case 1:
                        mark+= f1(mesArr, sentiments, sentiment);
                        break;
                    case 2:
                        mark += f2(mesArr, sentiments, sentiment);
                        break;
                    case 3:
                        mark += f3(mesArr, sentiments, sentiment);
                        break;
                    case 4:
                        mark += f4(mesArr, sentiments, sentiment);
                        break;
                    default:
                        
                        break;
                }
            }
            return mark;
        }
        public void CorrelateSentMes(List<string> message,Dictionary<string, float> sentiments, UserInfo UsIn)
        {

            //var sv = new Stopwatch();
            //sv.Start();
            int k = 0;
            Parallel.For(0, message.Count, i =>
            {
                    if(k%1000==0)
                    Console.WriteLine(k+"%1000==0");
                k += 1;
                var mesArr = message[i].Split(" ");
                    UsIn.marks[i] += GetMark(mesArr, sentiments);
            });
            //sv.Stop();
            //Console.WriteLine(sv.Elapsed);
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
