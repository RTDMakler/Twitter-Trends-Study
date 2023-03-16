using System.Data.Entity.Core.Objects.DataClasses;
using System.Diagnostics;
using Tweet_Trends;

namespace TweetTrends
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            //var sv = new Stopwatch();
            var states = new List<State>();
            new Parser().jsPars(states ,"states.json");
            //sv.Start();
            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv");
            var userInfo = new UserInfo("texas_tweets2014.txt");
            new Parser().ImInsideYou(userInfo,states);
          
            //foreach(var mark in marks)
            //    Console.WriteLine(mark);; ; ; ; ; ; ; ;
            //sv.Stop();
            //Console.WriteLine(sv.Elapsed);
        }
    }
}
