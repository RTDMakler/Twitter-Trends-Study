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
            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv");
            var userInfo = new UserInfo("tweets20111.txt");
            float [] marks =userInfo.GetMarks(sentiments);
            //foreach(var mark in marks)
            //    Console.WriteLine(mark);; ; ; ; ; ; ; ;
            sv.Stop();
            Console.WriteLine(sv.Elapsed);
        }
    }
}
