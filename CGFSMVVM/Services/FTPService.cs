using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// FTP Service.
    /// </summary>
    public static class FTPService
    {
        /// <summary>
        /// Ups the load FTPI mage.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="hotelCode">Hotel code.</param>
        /// <param name="resNo">Reservation no.</param>
        public static async void UpLoadFTPImage(byte[] image, string hotelCode, string resNo)
        {
            var time = DateTime.Now;

            try
            {

                FtpWebRequest req = (FtpWebRequest)WebRequest.Create($"ftp://chml.keells.lk:5001/GuestFeedback/Photos/{hotelCode}_{resNo}.txt");
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential("ftp_chml", "hp##2009");

                req.ContentLength = image.Length;
                Stream reqStream = req.GetRequestStream();
                await reqStream.WriteAsync(image, 0, image.Length).ConfigureAwait(false);
                reqStream.Close();

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error uploading image" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Uploads the image http post.
        /// </summary>
        /// <param name="imageBytes">Image bytes.</param>
        /// <param name="hotelCode">Hotel code.</param>
        /// <param name="resNo">Res no.</param>
        /// <param name="guestId">Guest identifier.</param>

        public static async void UploadImageHttpPost(byte[] imageBytes,string hotelCode, string resNo, string guestId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();

                string timeStamp = "";

                if(string.IsNullOrEmpty(guestId))
                {
                    timeStamp = DateTime.Now.Ticks.ToString();
                }
                else
                {
                    timeStamp = guestId;
                }


                form.Add(new ByteArrayContent(imageBytes, 0, imageBytes.Length), "profile_pic", $"{hotelCode}_{resNo}_{timeStamp}.txt");
                HttpResponseMessage response = await httpClient.PostAsync($"{Settings.FTPUri}Feedback/UploadGuestImage", form).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
                httpClient.Dispose();
                string sd = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(sd);
            }
            catch(Exception ex)
            {
                var properties = new Dictionary<string, string> { { "Hotel Code", hotelCode }, { "Reservation Number", resNo } };
                Crashes.TrackError(ex, properties);
                Console.WriteLine("Error uploading image"+ ex.StackTrace);
            }
        }

        /// <summary>
        /// Writes the logger http post.
        /// </summary>
        /// <param name="imageBytes">Image bytes.</param>
        public static async void WriteLoggerHttpPost(byte[] imageBytes)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();

                form.Add(new ByteArrayContent(imageBytes, 0, imageBytes.Length), "logger_txt", "Logger.txt");
                HttpResponseMessage response = await httpClient.PostAsync($"{Settings.FTPUri}Feedback/UploadGuestImage", form).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
                httpClient.Dispose();
                string sd = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(sd);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                Console.WriteLine("Error uploading image" + ex.StackTrace);
            }
        }

    }
}
