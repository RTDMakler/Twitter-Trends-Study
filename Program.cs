using System.Data.Entity.Core.Objects.DataClasses;
using System.Diagnostics;
using Tweet_Trends;

namespace TweetTrends
{
    class Program
    {
        
        static void Main(string[] args)
        {

            var sv = new Stopwatch();
            sv.Start();
            var states = new List<State>();
            sv.Stop();

            new Parser().jsPars(states ,"states.json");
       
            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv");
        
            var userInfo = new UserInfo("tweets20111.txt");
            
            userInfo.FillMarks(sentiments);
            //Console.WriteLine(userInfo.Message[120] + userInfo.marks[120]);

            //foreach(var mark in marks)
            //    Console.WriteLine(mark);; ; ; ; ; ; ; ;

        }
    }
}
