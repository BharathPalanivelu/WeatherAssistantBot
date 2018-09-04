using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAssistantBot
{

    public class Constants
    {
        /// <summary>
        /// End Point To Access the Yahoo Weather APi
        /// </summary>
        public const string YahooQueryEndpoint = @"https://query.yahooapis.com/v1/public/yql?q=select 
                                * from weather.forecast where woeid in (select woeid from geo.places(1)
                                where text=""{0}"")&format=json";

        public enum DialogSteps
        {
            NameStep,
            PlaceStep,
            GetServiceChoice,
            MainDialog

        }
     

    }
}
