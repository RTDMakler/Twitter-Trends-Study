using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Diagnostics;
using System.Xml.Serialization;
using static System.Windows.Forms.AxHost;

namespace GMap
{
    public partial class Form1 : Form
    {
        List <State>states;
        UserInfo userInfo;
        public Form1()
        {
            InitializeComponent();
        }

        internal Form1(List<State> states,UserInfo userInfo):base()
        {
            this.states = states;
            this.userInfo = userInfo;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        void GlobalMarks()
        {
            var sv = new Stopwatch();
            sv.Start();
            Parallel.ForEach(states, state =>
            {
                Parallel.ForEach(state.gMapPolygons, partOfState =>
                {
                    for (int i = 0; i < userInfo.marks.Length; i++)
                    {
                        if (partOfState.IsInside(new PointLatLng(userInfo.XPos[i], userInfo.YPos[i])))
                        {
                            state.stateSentiment += userInfo.marks[i];
                        }
                    }
                });
            });
            sv.Stop();
            Console.WriteLine(sv.Elapsed);
        }

        GMapOverlay GetMarkers(GMapOverlay markersOverlay)
        {
            GMarkerGoogle marker;
            int amountOfTweets = userInfo.marks.Length;
            amountOfTweets = amountOfTweets > 200000 ? 200000 : amountOfTweets;
            for (int i = 0; i < amountOfTweets; i++)
            {
                if (userInfo.marks[i] < 0 && userInfo.marks[i] > -25)
                {
                    marker = new GMarkerGoogle(new PointLatLng(userInfo.XPos[i], userInfo.YPos[i]),
              GMarkerGoogleType.purple_small);
                }
                else if (userInfo.marks[i] < 0)
                {
                    marker = new GMarkerGoogle(new PointLatLng(userInfo.XPos[i], userInfo.YPos[i]),
              GMarkerGoogleType.red_small);
                }
                else if (userInfo.marks[i] > 0 && userInfo.marks[i] < 50)
                {
                    marker = new GMarkerGoogle(new PointLatLng(userInfo.XPos[i], userInfo.YPos[i]),
              GMarkerGoogleType.yellow_small);
                }
                else
                {
                    marker = new GMarkerGoogle(new PointLatLng(userInfo.XPos[i], userInfo.YPos[i]),
              GMarkerGoogleType.green_small);
                }
                markersOverlay.Markers.Add(marker);
            }
            return markersOverlay;
        }
        GMapOverlay GetPolygons()
        {
            GMapOverlay polyOverlay = new GMapOverlay();
            foreach (var state in states)
            {
                foreach (var partOfState in state.gMapPolygons)
                {
                    polyOverlay.Polygons.Add(partOfState);

                    if (state.stateSentiment > 0)
                    {
                        int k = (int)(255 * (state.stateSentiment - (int)state.stateSentiment));
                        k = k > 255 ? 255 : k;
                        partOfState.Fill = new SolidBrush(Color.FromArgb(k / 10, k, 0));
                    }
                    else if (state.stateSentiment < 0)
                    {
                        int k = (int)(255 * ((state.stateSentiment * -1) - (int)(state.stateSentiment * -1)) * 60);
                        k = k > 255 ? 255 : k;
                        partOfState.Fill = new SolidBrush(Color.FromArgb(k, k / 10, 0));
                    }

                    partOfState.Stroke = new Pen(Color.Red, 1);

                }
            }
            return polyOverlay;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            double lat = Convert.ToDouble(/*textBox2.Text*/40);
            double lon = Convert.ToDouble(/*textBox1.Text*/-110.0);

            gMapControl1.Position = new GMap.NET.PointLatLng(lat, lon);

            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 50;
            gMapControl1.Zoom = 3;

            gMapControl1.MapScaleInfoEnabled = true;


            GlobalMarks();


            GMapOverlay markersOverlay = new GMapOverlay("markers");
            GetMarkers(markersOverlay);
            

            
            gMapControl1.Overlays.Add(GetPolygons());
            gMapControl1.Overlays.Add(markersOverlay);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}