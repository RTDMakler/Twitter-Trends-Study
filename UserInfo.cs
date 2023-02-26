using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweet_Trends
{
    internal class UserInfo
    {
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
            new ReadFromFile().GetInfo(link, XPos, YPos, Message);
        }
    }
}
