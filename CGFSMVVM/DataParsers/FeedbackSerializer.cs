using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace CGFSMVVM.DataParsers
{
    /// <summary>
    /// Guest Feedback data serializer.
    /// </summary>
    public static class FeedbackSerializer
    {

        private static String ratingListJson;
        private static String commentListJson;
        private static String otherListJason;

        static NameValueCollection ratingNVC;
        static NameValueCollection commentNVC;
        static NameValueCollection otherNVC;

        private static bool isSetRating = false;
        private static bool isSetComment = false;
        private static bool isSetOtherQ = false;

        /// <summary>
        /// Initializes the namevalu collections
        /// </summary>
        static FeedbackSerializer()
        {
            ratingNVC = new NameValueCollection();
            commentNVC = new NameValueCollection();
            otherNVC = new NameValueCollection();
        }

        /// <summary>
        /// Generates the feedback POST Json.
        /// </summary>
        /// <returns>The feedback POST Json.</returns>
        public static string GetFeedbackPOSTJson()
        {
            try
            {
                bool isJsonStringsCreated = CreateJsonStrings();

                bool isJsonArraysCreated = CreateJSonArrays();

                var cartData = FeedbackCart.GetMainReservationDetailsFromCart();

                if (isJsonArraysCreated && isJsonStringsCreated)
                {
                    string json = "{" +
                        "\"HtlCode\":\"" + cartData.HtlCode + "\"," +
                       "\"RoomNo\":\"" + cartData.RoomNo + "\"," +
                       "\"ReservationNo\":\"" + cartData.ReservationNo + "\"," +
                       "\"GuestID\":\"" + cartData.GuestID + "\"," +
                       "\"Country\":\"" +cartData.Country + "\"," +
                       "\"GuestName\":\"" + cartData.GuestName + "\"," +
                       "\"GuestEmail\":\"" + cartData.GuestEmail + "\"," +
                       "\"GuestAddress\":\"" + cartData.GuestAddress + "\"," +
                       "\"GuestPhone\":\"" + cartData.GuestPhone + "\"," +
                       "\"ArrivalDate\":\"" + cartData.ArrivalDate + "\"," +
                       "\"DepartureDate\":\"" + cartData.DepartureDate + "\"," +
                       "\"CreatedBy\":\"" + cartData.CreatedBy + "\"," +
                       "\"StartTime\":\"" + cartData.StartTime + "\"," +
                       "\"EndTime\":\"" + cartData.EndTime + "\"," +
                       "\"MainCategoryId\":" + cartData.MainCategoryId + "," +
                       "\"RatingList\":[" + ratingListJson + "]," +
                       "\"CommentList\":[" + commentListJson + "]," +
                       "\"OtherList\":[" + otherListJason + "]}";

                    Console.WriteLine(json);

                    return json;
                }

                return null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
        }

        /// <summary>
        /// Creates the json strings.
        /// </summary>
        /// <returns><c>true</c>, if json strings was created, <c>false</c> otherwise.</returns>
        private static bool CreateJsonStrings()
        {

            NameValueCollection ratingCollection = FeedbackCart.RatingNVC;

            for (int i = 0; i < ratingCollection.Count; i++)
            {
                String _RJson =
                                "{\"CommID\":0," +
                                "\"QId\":" + ratingCollection.GetKey(i) + "," +
                                "\"Rating\":" + ratingCollection.Get(i) +
                                "}";

                ratingNVC.Add(ratingCollection.GetKey(i), _RJson);
            }

            NameValueCollection commentCollection = FeedbackCart.CommentNVC;

            for (int i = 0; i < commentCollection.Count; i++)
            {
                String _CommJson =
                                "{\"CommID\":0," +
                                "\"QId\":" + commentCollection.GetKey(i) + "," +
                                "\"Comment\":\"" + commentCollection.Get(i) +
                                "\"}";

                commentNVC.Add(commentCollection.GetKey(i), _CommJson);
            }

            NameValueCollection otherQCollection = FeedbackCart.OtherNVC;

            for (int i = 0; i < otherQCollection.Count; i++)
            {
                String _OqJson =
                                "{\"CommID\":0," +
                                "\"QId\":" + otherQCollection.GetKey(i) + "," +
                                "\"OQID\":" + otherQCollection.Get(i) +
                                "}";

                otherNVC.Add(otherQCollection.GetKey(i), _OqJson);
            }

            return true;
        }

        /// <summary>
        /// Creates the JSon arrays.
        /// </summary>
        /// <returns><c>true</c>, if JS on arrays was created, <c>false</c> otherwise.</returns>
        private static bool CreateJSonArrays()
        {
            try
            {
                if (FeedbackCart.RatingNVC != null && FeedbackCart.OtherNVC != null && FeedbackCart.CommentNVC != null)
                {
                    //Create Rating List JSON
                    for (int i = 0; i < ratingNVC.Count; i++)
                    {
                        if (isSetRating)
                        {
                            ratingListJson = ratingListJson + "," + ratingNVC.Get(i);
                        }
                        else
                        {
                            ratingListJson += ratingNVC.Get(i);
                            isSetRating = true;
                        }
                    }

                    //Create Comment List JSON
                    for (int i = 0; i < commentNVC.Count; i++)
                    {
                        if (isSetComment)
                        {
                            commentListJson = commentListJson + "," + commentNVC.Get(i);
                        }
                        else
                        {
                            commentListJson += commentNVC.Get(i);
                            isSetComment = true;
                        }
                    }

                    //Create Other Question List JSON
                    for (int i = 0; i < otherNVC.Count; i++)
                    {
                        if (isSetOtherQ)
                        {
                            otherListJason = otherListJason + "," + otherNVC.Get(i);
                        }
                        else
                        {
                            otherListJason += otherNVC.Get(i);
                            isSetOtherQ = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return false;
            }

            Console.WriteLine(ratingListJson);
            Console.WriteLine(commentListJson);
            Console.WriteLine(otherListJason);
            return true;
        }

        /// <summary>
        /// Resets the NVC s and json arrays,strings.
        /// </summary>
        public static void ResetRatingNVCs()
        {
            ratingNVC = new NameValueCollection();
            commentNVC = new NameValueCollection();
            otherNVC = new NameValueCollection();

            ratingListJson = null;
            commentListJson = null;
            otherListJason = null;

            isSetRating = false;
            isSetComment = false;
            isSetOtherQ = false;
        }
    }
}
