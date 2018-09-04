using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace WeatherAssistantBot
{
    public class WeatherHelper
    {

        public async Task<string> GetWeatherReport(string place, int option)
        {
            string weatherResponse = string.Empty;
            HttpClient client = new HttpClient();

            string uri = String.Format(Constants.YahooQueryEndpoint, place);
            HttpResponseMessage resp = await client.GetAsync(uri);
            var response = resp.Content.ReadAsStringAsync();

            WeatherApiResponse weatherApiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(response.Result);

            switch (option)
            {
                case 1: weatherResponse = GetTodaysWeather(weatherApiResponse);
                    break;
                case 2: weatherResponse = GetForecast(weatherApiResponse);
                    break;

                default: 
                    break;
            }

            return weatherResponse;
        }

        /// <summary>
        /// Function To Create a string response for Today's Weather
        /// </summary>
        /// <param name="apiResponse"></param>
        /// <returns></returns>
        private string GetTodaysWeather(WeatherApiResponse apiResponse)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( string.Format("Following is the Weather Report for {0},{1},{2} for Today {3}\n",
                                        apiResponse.Query.Results.Channel.Location.City, apiResponse.Query.Results.Channel.Location.Region,
                                        apiResponse.Query.Results.Channel.Location.Country,apiResponse.Query.Created.ToShortDateString()));
            stringBuilder.Append(String.Format("Wind Speed: {0}mph \n Humidity: {1}% \n Pressure: {2}in \n Sunrise: {3} \n Sunset: {4} \n " +
                                                "Temperature: {5} degF \n Overall Condition: {6} \n",
                                                apiResponse.Query.Results.Channel.Wind.Speed, apiResponse.Query.Results.Channel.Atmosphere.Humidity,
                                                apiResponse.Query.Results.Channel.Atmosphere.Pressure, apiResponse.Query.Results.Channel.Astronomy.Sunrise,
                                                apiResponse.Query.Results.Channel.Astronomy.Sunset, 
                                                apiResponse.Query.Results.Channel.Item.Condition.Temp, apiResponse.Query.Results.Channel.Item.Condition.Text));

            stringBuilder.Append("Thank you for using WeatherAssitantBot");
            return stringBuilder.ToString();
        }


        /// <summary>
        /// Function To Parse a forecast for 10 days
        /// </summary>
        /// <param name="apiResponse"></param>
        /// <returns></returns>
        private string GetForecast(WeatherApiResponse apiResponse)
        {
            List<Forecast> forecasts = apiResponse.Query.Results.Channel.Item.Forecast;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("Following is the Forecast for {0},{1},{2} for next ten days. \n",
                                 apiResponse.Query.Results.Channel.Location.City, apiResponse.Query.Results.Channel.Location.Region,
                                 apiResponse.Query.Results.Channel.Location.Country));
            
            
            //Navigate Through Each Day Forecast object
            foreach (Forecast forecast in forecasts)
            {
                stringBuilder.Append(string.Format("Date: {0}, {1}", forecast.Day, forecast.Date));
                stringBuilder.Append("\n");
                stringBuilder.Append(string.Format("Max Temperature: {0}F \n Min Temperature: {1}F \n", forecast.High, forecast.Low));
                stringBuilder.Append(string.Format("Overall Condition : {0}\n \n", forecast.Text));
                
                
            }
            stringBuilder.Append("Thank You for using Weather Assistant");


            return stringBuilder.ToString();
        }

    }
}
