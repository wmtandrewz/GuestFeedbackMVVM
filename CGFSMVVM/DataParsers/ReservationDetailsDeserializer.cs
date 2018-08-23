using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using Newtonsoft.Json;

namespace CGFSMVVM.DataParsers
{
    /// <summary>
    /// Reservation details deserializer.
    /// </summary>
    public static class ReservationDetailsDeserializer
    {
        private static Dictionary<string, ReservationDetailsModel> ReservationDetailsDictionary = new Dictionary<string, ReservationDetailsModel>();

        /// <summary>
        /// Deserializes the reservation details.
        /// </summary>
        /// <returns>The reservation details. <see cref="T:CGFSMVVM.Models.ReservationDetailsModel"/></returns>
        /// <param name="hotelNumber">Hotel number.</param>
        /// <param name="roomNo">Room no.</param>
        /// <param name="date">Inhouse Date.</param>
        public static async Task<Dictionary<string, ReservationDetailsModel>> DeserializeReservationDetails(string hotelNumber,string roomNo,string date)
        {
            ReservationDetailsDictionary.Clear();

            string _result = await APIGetServices.GetReservationDetailsFromAPI(hotelNumber, roomNo, date).ConfigureAwait(true);

            Console.WriteLine(_result.Contains("Resource not found"));

            if (!_result.Contains("Resource not found"))
            {
                List<ReservationDetailsModel> ReservationDetailList = JsonConvert.DeserializeObject<List<ReservationDetailsModel>>(_result);

                if (ReservationDetailList != null)
                {
                    int sequence = 1;

                    foreach (var item in ReservationDetailList)
                    {
                        ReservationDetailsDictionary.Add(sequence.ToString(), item);
                        sequence++;
                    }

                    return ReservationDetailsDictionary;
                }
                else
                {
                    return null;
                }
            }

            return null;

        }
    }
}
