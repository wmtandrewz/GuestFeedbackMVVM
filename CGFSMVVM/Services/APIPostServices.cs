using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Helpers;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// API POST services.
    /// </summary>
    public static class APIPostServices
    {

        /// <summary>
        /// Saves the feedback data.
        /// </summary>
        /// <returns>The feedback POST service responce</returns>
        public static async Task<bool> SaveFeedbackData()
        {
            using (var client = new WebClient())
            {
                try
                {
                    string json = FeedbackSerializer.GetFeedbackPOSTJson();
                    bool responce = false;

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    //client.Headers.Add("Ocp-Apim-Subscription-Key", Settings.SubscriptionKey);
                    //client.Headers.Add("Ocp-Apim-Subscription-Key","d0fbb5e7bebc454e8df6ff295fa73905");

                    string apiResponce = await client.UploadStringTaskAsync(new Uri(Settings.BaseDomainURL + "Feedback/Insert"), "POST", json);//Live
                    //client.UploadStringAsync(new Uri("https://jkhapimdev.azure-api.net/api/beta/v1/" + "guestfeedback/Feedback/Insert"), "POST", json);
                    Console.WriteLine(apiResponce);

                    client.UploadStringCompleted += (object sender, UploadStringCompletedEventArgs e) =>
                    {

                        Console.WriteLine(e.Result);
                        var res = e.Result;

                        if (res.All(char.IsDigit))
                        {
                            responce = true;
                            FeedbackSerializer.ResetRatingNVCs();
                        }

                    };

                    FeedbackSerializer.ResetRatingNVCs();
                    return responce;

                }
                catch (Exception) 
                {
                    FeedbackSerializer.ResetRatingNVCs();
                    return false;
                }

            }
        }

    }
}
