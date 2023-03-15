using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweet_Trends
{
    internal class State
    {
        public List<GMapPolygon> gMapPolygons= new List<GMapPolygon>();
        public double stateSentiment;
        public string stateName;

    }
}
