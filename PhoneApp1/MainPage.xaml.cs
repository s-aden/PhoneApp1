using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using System.Device.Location;
using System.Device;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;



namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
       public MapLayer Glayer = new MapLayer();
       public MapLayer Dlayer = new MapLayer();
       public MapLayer Hlayer = new MapLayer();
       public MapLayer Vlayer = new MapLayer();
       public MapLayer Ulayer = new MapLayer();

       public MapLayer backupG = new MapLayer();
       public MapLayer backupD = new MapLayer();
       public MapLayer backupH = new MapLayer();
       public MapLayer backupV = new MapLayer();
       public MapLayer backupU = new MapLayer();

       public List<Pushpin> listGpin = new List<Pushpin>();
       public List<Pushpin> listDpin = new List<Pushpin>();
       public List<Pushpin> listHpin = new List<Pushpin>();
       public List<Pushpin> listVpin = new List<Pushpin>();
       public List<Pushpin> listUpin = new List<Pushpin>();

       public int g = 0;
       public int d = 0;
       public int h = 0;
       public int v = 0;
       public int u = 0;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            startGPS();
            
            setMap();

/*                MapPolyline circle = new MapPolyline();
                circle.Path = CreateCircle(mainMap.Center, 3);
                circle.StrokeColor = Colors.Red;
                circle.StrokeThickness = 3;
                MapOverlay circleOverlay = new MapOverlay();
                circleOverlay.Content = circle;
                mainMap.Layers.Add(new MapLayer { circleOverlay });
*/
            //webquery();
            dataPage();
        }

        private void dataPage()
        {
            List<String> data = new List<String>();
            String[] issues = new String[] { "Grafitti", "Dog Poop", "Human Excretion", "Vomitting", "Urine" };
            int[] numbers = new int[] { g, d, h, v, u };
            for (int i = 0; i < issues.Length; i++)
            {
                data.Add(issues[i] + " total: " + numbers[i]);

            }

            dataList.ItemsSource = data;
            
        }

    /*    private async void webquery()
        {

            StringContent content;
            var json = new DataContractJsonSerializer(typeof(DTOPoint));
            // use the serializer to write the object to a MemoryStream 
            var ms = new MemoryStream();
            json.WriteObject(ms, new DTOPoint()
            {
                Centre =  new point() { latitude=mainMap.Center.Latitude , longitude =mainMap.Center.Longitude},
                Points =  {new point() {latitude=listDpin[0].GeoCoordinate.Latitude,longitude=listDpin[0].GeoCoordinate.Longitude }, new point() {latitude=listVpin[0].GeoCoordinate.Latitude,longitude= listVpin[0].GeoCoordinate.Longitude } },
                Radius = 400,
            });
            ms.Position = 0;
            //use a Stream reader to construct the StringContent (Json) 
            using (var reader = new StreamReader(ms))
            {
                content = new StringContent(reader.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsync("http://blub.azurewebsites.net/api/Values", content);

        } */


        public  void setMap()
        {

            StreamReader reader = new StreamReader("text.txt");
            String line;
            List<Reports> listGraf = new List<Reports>();
            List<Reports> listDog = new List<Reports>();
            List<Reports> listHuman = new List<Reports>();
            List<Reports> listVomit = new List<Reports>();
            List<Reports> listUrine = new List<Reports>();
            




            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line.StartsWith("Graf"))
                {

                    listGraf.Add(new Reports(line));

                    listGpin.Add(new Pushpin { Content = "Graffiti", Background = new System.Windows.Media.SolidColorBrush(Colors.Green) });
                    Glayer.Add(new MapOverlay { Content = listGpin.ElementAt(g) });
                    Glayer[g].GeoCoordinate = new GeoCoordinate(listGraf.ElementAt(g).Latitude, listGraf.ElementAt(g).Longitude);
                    
                    g++;
                }
                else if (line.StartsWith("Dog"))
                {
                    listDog.Add(new Reports(line));

                    listDpin.Add(new Pushpin { Content = "Dog Poop", Background = new System.Windows.Media.SolidColorBrush(Colors.Brown),  });
                    Dlayer.Add(new MapOverlay { Content = listDpin.ElementAt(d) });
                    Dlayer[d].GeoCoordinate = new GeoCoordinate(listDog.ElementAt(d).Latitude, listDog.ElementAt(d).Longitude);
                    
                    d++;
                }
                else if (line.StartsWith("Human"))
                {
                    listHuman.Add(new Reports(line));

                    listHpin.Add(new Pushpin { Content = "Human", Background = new System.Windows.Media.SolidColorBrush(Colors.Cyan), Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black) });
                    Hlayer.Add(new MapOverlay { Content = listHpin.ElementAt(h)  });
                    Hlayer[h].GeoCoordinate = new GeoCoordinate(listHuman.ElementAt(h).Latitude, listHuman.ElementAt(h).Longitude);
                   
                    h++;
                }
                else if (line.StartsWith("Vom"))
                {
                    listVomit.Add(new Reports(line));
                    Vlayer.Add(new MapOverlay { Content = new Pushpin { Content = "Vomit", Background = new System.Windows.Media.SolidColorBrush(Colors.Yellow), Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black) } });
                    Vlayer[v].GeoCoordinate = new GeoCoordinate(listVomit.ElementAt(v).Latitude, listVomit.ElementAt(v).Longitude);
                    
                    v++;
                }
                else if (line.StartsWith("Urin"))
                {
                    listUrine.Add(new Reports(line));
                    Ulayer.Add(new MapOverlay { Content = new Pushpin {Content = "Urine", Background = new System.Windows.Media.SolidColorBrush(Colors.Purple)}});
                    Ulayer[u].GeoCoordinate = new GeoCoordinate(listUrine.ElementAt(u).Latitude, listUrine.ElementAt(u).Longitude);
                    
                    u++;
                }

            }
            mainMap.Layers.Add(Glayer);
            mainMap.Layers.Add(Dlayer);
            mainMap.Layers.Add(Hlayer);
            mainMap.Layers.Add(Vlayer);
            mainMap.Layers.Add(Ulayer);
        }

        private void startGPS()
        {
            GeoCoordinateWatcher pos = new GeoCoordinateWatcher();
            pos.TryStart(false, TimeSpan.FromMilliseconds(1000));
            if (pos.Position.Location.IsUnknown != false)
            {
                MessageBox.Show("In order to use this app please enable location");
            }
            else
            {
                mainMap.Center = pos.Position.Location;
                mainMap.ZoomLevel = 16;



                Ellipse myCircle = new Ellipse();
                myCircle.Fill = new SolidColorBrush(Colors.Blue);
                myCircle.Height = 20;
                myCircle.Width = 20;
                myCircle.Opacity = 50;

                MapOverlay myLocationOverlay = new MapOverlay();
                myLocationOverlay.Content = myCircle;
                myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                myLocationOverlay.GeoCoordinate = pos.Position.Location;

                MapLayer myLocationLayer = new MapLayer();
                myLocationLayer.Add(myLocationOverlay);
                mainMap.Layers.Add(myLocationLayer);

            }
        }

        private static double ToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }

        private static double ToDegrees(double radian)
        {
            return radian * (180 / Math.PI);
        }

        public static GeoCoordinateCollection CreateCircle(GeoCoordinate center, double radius)
        {
            var earthRadius = 6367.0; // radius in kilometers
            var lat = ToRadian(center.Latitude); //radians
            var lng = ToRadian(center.Longitude); //radians
            var d = radius / earthRadius; // d = angular distance covered on earth's surface
            var locations = new GeoCoordinateCollection();
 
            for (var x = 0; x <= 360; x++)
            {
            var brng = ToRadian(x);
            var latRadians = Math.Asin(Math.Sin(lat) * Math.Cos(d) + Math.Cos(lat) * Math.Sin(d) * Math.Cos(brng));
            var lngRadians = lng + Math.Atan2(Math.Sin(brng) * Math.Sin(d) * Math.Cos(lat), Math.Cos(d) - Math.Sin(lat) * Math.Sin(latRadians));
            locations.Add(new GeoCoordinate(ToDegrees(latRadians), ToDegrees(lngRadians)));
            }
 
          return locations;
        }

        private void grafCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<MapOverlay> backup = new List<MapOverlay>();
                
                backup = backupG.ToList();
                backupG.Clear();
                for (int i = 0; i < backup.Capacity; i++)
                {
                    Glayer.Add(backup[i]);
                }
                backup.Clear();
                
            }
            catch (NullReferenceException i)
            {
            }
        }

        private void vomCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<MapOverlay> backup = new List<MapOverlay>();
                
                backup = backupV.ToList();
                backupV.Clear();
                for (int i = 0; i < backup.Capacity; i++)
                {
                    Vlayer.Add(backup[i]);
                }
                backup.Clear();
                
            }
            catch (NullReferenceException i)
            {
            }
                    
        }

        private void humCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<MapOverlay> backup = new List<MapOverlay>();

                backup = backupH.ToList();
                backupH.Clear();
                for (int i = 0; i < backup.Capacity; i++)
                {
                    Hlayer.Add(backup[i]);
                }
                backup.Clear();

            }
            catch (NullReferenceException i)
            {
            }
                    
        }

        private void dogCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<MapOverlay> backup = new List<MapOverlay>();

                backup = backupD.ToList();
                backupD.Clear();
                for (int i = 0; i < backup.Capacity; i++)
                {
                    Dlayer.Add(backup[i]);
                }
                backup.Clear();

            }
            catch (NullReferenceException i)
            {
            }
                    
        }

        private void urineCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<MapOverlay> backup = new List<MapOverlay>();

                backup = backupU.ToList();
                backupU.Clear();
                for (int i = 0; i < backup.Capacity; i++)
                {
                    Ulayer.Add(backup[i]);
                }
                backup.Clear();

            }
            catch (NullReferenceException i)
            {
            }
                    
        }

        private void grafCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            List<MapOverlay> backup = new List<MapOverlay>();
            backup = Glayer.ToList();
            Glayer.Clear();


            
            
            for (int i = 0; i < backup.Capacity; i++)
            {
                backupG.Add(backup[i]);
            }

        }

        private void vomCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            List<MapOverlay> backup = new List<MapOverlay>();
            backup = Vlayer.ToList();
            Vlayer.Clear();




            for (int i = 0; i < backup.Capacity; i++)
            {
                backupV.Add(backup[i]);
            }
        }

        private void dogCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            List<MapOverlay> backup = new List<MapOverlay>();
            backup = Dlayer.ToList();
            Dlayer.Clear();




            for (int i = 0; i < backup.Capacity; i++)
            {
                backupD.Add(backup[i]);
            }
        }

        private void humCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            List<MapOverlay> backup = new List<MapOverlay>();
            backup = Hlayer.ToList();
            Hlayer.Clear();




            for (int i = 0; i < backup.Capacity; i++)
            {
                backupH.Add(backup[i]);
            }
        }

        private void urineCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            List<MapOverlay> backup = new List<MapOverlay>();
            backup = Ulayer.ToList();
            Ulayer.Clear();




            for (int i = 0; i < backup.Capacity; i++)
            {
                backupU.Add(backup[i]);
            }
        }

        private void mainMap_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate newCenter = mainMap.Center;
            mainMap.MaxHeight = 1280;
            mainMap.MaxWidth = 768;
        }



        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }

    public class point
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
    }

    public class DTOPoint
    {
        public List<point> Points { get; set; }
        public point Centre { get; set; }
        public int Radius { get; set; }
    }
} 