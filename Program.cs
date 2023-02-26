using Tweet_Trends;

namespace TweetTrends
{
    class Program
    {
        static void Main(string[] args)
        {
            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv");
            var userInfo = new UserInfo("texas_tweets2014.txt");
			#deleteline
        }
    }
}
