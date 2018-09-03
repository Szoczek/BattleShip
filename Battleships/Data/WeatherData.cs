using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Data
{
    public class WeatherData
    {
        //config
        private static IRestResponse<WeatherResponse> Response { get; set; }
        private RestClient Client = new RestClient("http://api.openweathermap.org");
        private RestRequest Request = new RestRequest("/data/2.5/weather");
        private string Localization { get; set; }

        //readable
        public string windDirection { get; set; }
        public string windSpeed { get; set; }

        public WeatherData(string location)
        {
            Localization = location;
            WeatherInfo();
            windDirection = WindDirection(Response.Data.Wind.Deg).ToString() + $"({(Response.Data.Wind.Deg).ToString()})";
            windSpeed = Math.Round(Response.Data.Wind.Speed) + " m/s";

        }

        public IRestResponse<WeatherResponse> GetWeatherResponse()
        {
            return Response;
        }
        

        public void WeatherInfoUpdate()
        {
            Response = Client.Execute<WeatherResponse>(Request);
        }

        public enum WindDir
        {
            North, // 0st
            NorthEast, //35st
            East, //90st
            SouthEast, //125st
            South, //180st
            SouthWest, //215st
            West, //270st
            NorthWest //305st
        }

        public WindDir WindDirection(double degree)
        {
            if ((degree > 337.5) || (degree <= 22.5))
                return WindDir.North;
            if ((degree > 22.5) && (degree <= 67.5))
                return WindDir.NorthEast;
            if ((degree > 67.5) && (degree <= 112.5))
                return WindDir.East;
            if ((degree > 112.5) && (degree <= 147.5))
                return WindDir.SouthEast;
            if ((degree > 147.5) && (degree <= 192.5))
                return WindDir.South;
            if ((degree > 192.5) && (degree <= 237.5))
                return WindDir.SouthWest;
            if ((degree > 237.5) && (degree <= 282.5))
                return WindDir.West;
            return WindDir.NorthWest;
        }

        private void WeatherInfo()
        {
            Request.AddParameter("q", Localization);
            Request.AddParameter("appid", "df274b3c7455f4d2c39c5474cd35b169");
            Response = Client.Execute<WeatherResponse>(Request);
        }
    }

    public class WeatherResponse
    {
        public WeatherResponseWind Wind { get; set; }
    }

    public class WeatherResponseWind
    {
        public double Speed { get; set; }
        public double Deg { get; set; }
    }

}
