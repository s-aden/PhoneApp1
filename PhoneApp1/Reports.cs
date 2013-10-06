using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1
{
    class Reports
    {
        public double Longitude;
        public double Latitude;
        public string Type;
        public Reports(string type, double latitude, double longitude)
        {
            Type = type;
            Longitude = longitude;
            Latitude = latitude;
        }

        public Reports(string CSVLine)
        {
            String[] ray = CSVLine.Split(',');
            Type = ray[0];
            Latitude = double.Parse(ray[1]);
            Longitude = double.Parse(ray[2]);
        }

       
    }
}
