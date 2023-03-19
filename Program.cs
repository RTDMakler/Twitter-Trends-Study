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
            //sv.Start();
            var states = new List<State>(); 
            
            new Parser().jsPars(states ,"states.json"); Console.WriteLine("Created array of states");
            //sv.Stop();
            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv"); Console.WriteLine("Got sentiments");

            var userInfo = new UserInfo("tweets20111.txt"); Console.WriteLine("Got UserInfo");

            userInfo.FillMarks(sentiments); Console.WriteLine("Filled markes");
           
            //Console.WriteLine(sv.Elapsed);
        }
    }
}
