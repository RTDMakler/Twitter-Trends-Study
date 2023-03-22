using System;

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GMap
{
    internal class UserInfo
    {
        public float[] marks;
        private List<float> xPos;

        public List<float> XPos
        {
            get { return xPos; }
            private set { xPos = value; }
        }

        private List<float> yPos;

        public List<float> YPos
        {
            get { return yPos; }
            private set { yPos = value; }
        }
        private List<string> message;

        public List<string> Message
        {
            get { return message; }
            private set { message = value; }
        }
        public UserInfo(string link) 
        {
            YPos = new List<float>();
            XPos = new List<float>();
            Message = new List<string>();
            new ReadFromFile().GetInfo(link, this);
            marks= new float[Message.Count];
        }
        public void FillTweet(float x, float y,string mes)
        {
            XPos.Add(x); YPos.Add(y); Message.Add(mes);
        }
        public void FillMarks(Dictionary<string, float> sentiments)
        {
            new Parser().CorrelateSentMes(message, sentiments, this);
        }
    }
}
