using static System.Windows.Forms.AxHost;

namespace GMap
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            
            //var sv = new Stopwatch();
            //sv.Start();
            var states = new List<State>();

            new Parser().jsPars(states, "states.json"); Console.WriteLine("Created array of states");

            var sentiments = new ReadFromFile().GetSentiments("sentiments.csv"); Console.WriteLine("Got sentiments");

            var userInfo = new UserInfo("tweets20111.txt"); Console.WriteLine("Got UserInfo");

            userInfo.FillMarks(sentiments); Console.WriteLine("Filled markes");
            //sv.Stop();
            //Console.WriteLine(sv.Elapsed);


            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(states, userInfo));
        }

    }
}