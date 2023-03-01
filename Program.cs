using Tweet_Trends;

namespace TweetTrends
{
    class Program
    {
        static void Main(string[] args)
        {
            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv");
            var userInfo = new UserInfo("texas_tweets2014.txt");
            float [] marks =userInfo.GetMarks(sentiments);
            foreach(var mark in marks)
                Console.WriteLine(mark);
        }
    }
}
