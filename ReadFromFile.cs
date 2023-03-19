using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tweet_Trends
{
    internal class ReadFromFile
    {
        private ReadOnlySequence<byte> jsonUtf8Bytes;

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
        public void GetInfo(string link,UserInfo UsInfo)
        {
            using (StreamReader reader = new StreamReader(link))
            {
                string line;
                var pars = new Parser();
                Parallel.For(0, System.IO.File.ReadAllLines(link).Length, i =>
                {
                    line = reader.ReadLine();
                    pars.ParseLocMes(line, UsInfo);
                });
            }
        }
    }
}
