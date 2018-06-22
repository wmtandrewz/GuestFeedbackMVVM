using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using Microsoft.AppCenter.Crashes;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// API GET services.
    /// </summary>
    public static class APIGetServices
    {
        /// <summary>
        /// Gets the questions from API.
        /// </summary>
        /// <returns>The questions from API.</returns>
        /// <param name="hotelcode">Hotelcode.</param>
        /// <param name="lang">Language</param>
        /// <param name="area">DB area</param>
        public static async Task<string> GetQuestionsFromAPI(string hotelcode, string lang, string area)
        {

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(Settings.BaseDomainURL);
            //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.SubscriptionKey); 
            //var response = await client.GetAsync("guestfeedback/HotelQuestions/GetHotelQuestions/" + hotelcode + "/" + lang + "/" + area); 

            //var resultQues = response.Content.ReadAsStringAsync().Result;

            //if (!string.IsNullOrEmpty(resultQues))
            //{
            //    Console.WriteLine("Question JSON Result Recieved...");

            //    return resultQues;

            //}
            //else
            //{
            //    return "";
            //}

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Settings.BaseDomainURL);
            var response = await client.GetAsync("HotelQuestions/GetHotelQuestions/" + hotelcode + "/" + lang);

            var resultQues = response.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(resultQues))
            {
                Console.WriteLine("Question JSON Result Recieved...");

                return resultQues;

            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the reservation details from API.
        /// </summary>
        /// <returns>The reservation details from API.</returns>
        /// <param name="hotelNumber">Hotel number.</param>
        /// <param name="roomNo">Room no.</param>
        /// <param name="date">Date.</param>
        public static async Task<string> GetReservationDetailsFromAPI(string hotelNumber, string roomNo, string date)
        {
            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(Settings.BaseDomainURL);
                //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.SubscriptionKey);
                client.BaseAddress = new Uri("https://cheetah.azure-api.net/api/v1/");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "c96b7f401241458290ce8544207eb43d"); //Production link
                var response = await client.GetAsync("guestfeedback/Guest/GetGuestDetails/" + hotelNumber + "/" + roomNo + "/" + date);

                var resultData = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(resultData))
                {
                    Console.WriteLine("JSON Reservation Result Deserialized...List Size :" + resultData.Count());

                    return resultData;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string> { { "Hotel Number", hotelNumber }, { "Room", roomNo }, { "Date", date } };
                Crashes.TrackError(exception, properties);
                return "";
            }
        }

        /// <summary>
        /// Gets the is feedback given.
        /// </summary>
        /// <returns>The is feedback given.</returns>
        /// <param name="hotelCode">Hotel code.</param>
        /// <param name="resNo">Reservation no.</param>
        /// <param name="guestID">Guest identifier.</param>
        public static async Task<string> GetIsFeedbackGiven(string hotelCode, string resNo, string guestID)
        {
            string resultData = null;

            try
            {

                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(Settings.BaseDomainURL);
                //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.SubscriptionKey);
                client.BaseAddress = new Uri("https://cheetah.azure-api.net/api/v1/");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "c96b7f401241458290ce8544207eb43d");
                var response = await client.GetAsync("guestfeedback/Feedback/IsFeedbackGiven/" + hotelCode + "/" + resNo + "/" + guestID).ConfigureAwait(true);
                resultData = response.Content.ReadAsStringAsync().Result;

                if (resultData != "")
                {
                    Console.WriteLine("Is Feedback given : " + resultData);

                    return resultData;
                }
                else
                {
                    return resultData;
                }
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string> { { "Hotel Code", hotelCode }, { "Reservation Number", resNo }, { "Guest", guestID } };
                Crashes.TrackError(exception, properties);
                return resultData;
            }
        }

    }
}
