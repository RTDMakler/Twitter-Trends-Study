using System.Data.Entity.Core.Objects.DataClasses;
using System.Diagnostics;
using Tweet_Trends;

namespace TweetTrends
{
    class Program
    {
        static void Main(string[] args)
        {

            //var sv = new Stopwatch();
            var states = new List<State>();
            new Parser().jsPars(states ,"states.json");
            //sv.Start();
            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv");
            var userInfo = new UserInfo("tweets20111.txt");
            float [] marks =userInfo.GetMarks(sentiments);
            //foreach(var mark in marks)
            //    Console.WriteLine(mark);; ; ; ; ; ; ; ;
            //sv.Stop();
            //Console.WriteLine(sv.Elapsed);
        }
    }
}
