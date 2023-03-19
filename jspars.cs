using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

using GMap.NET;
using GMap.NET.WindowsForms;

namespace Tweet_Trends
{
    internal class jspars
    {
        List <GMapPolygon> gMapPolygons= new List <GMapPolygon> ();
        List <PointLatLng> pointLatLngs= new List <PointLatLng> (); 
    }   
}
