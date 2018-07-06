using System;
using System.Collections.Specialized;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// Feedback cart.
    /// </summary>
    public static class FeedbackCart
    {
        public static string _hotelIdentifier { get; set; }
        public static string _roomNum { get; set; }
        public static string _resNum { get; set; }
        public static string _guestID { get; set; }
        public static string _country { get; set; }
        public static string _guestName { get; set; }
        public static string _guestEmail { get; set; }
        public static string _guestAddress { get; set; }
        public static string _guestPhone { get; set; }
        public static byte[] _guestImage { get; set; }
        public static string _arrDate { get; set; }
        public static string _depDate { get; set; }
        public static string _createdBy { get; set; }
        public static string _startTime { get; set; }
        public static string _endTime { get; set; }
        public static int _mainCatId { get; set; }

        public static NameValueCollection RatingNVC { get; set; }
        public static NameValueCollection CommentNVC { get; set; }
        public static NameValueCollection OtherNVC { get; set; }

        static FeedbackCart()
        {
            RatingNVC = new NameValueCollection();
            CommentNVC = new NameValueCollection();
            OtherNVC = new NameValueCollection();

        }

        /// <summary>
        /// Clears the saved data.
        /// </summary>
        public static void ClearSavedData()
        {
            Console.WriteLine("Cleared feedback Cart Data");

            RatingNVC = new NameValueCollection();
            CommentNVC = new NameValueCollection();
            OtherNVC = new NameValueCollection();

            _hotelIdentifier = null;
            _roomNum = null;
            _resNum = null;
            _guestID = null;
            _country = null;
            _guestName = null;
            _guestEmail = null;
            _guestAddress = null;
            _guestPhone = null;
            _guestImage = null;
            _arrDate = null;
            _depDate = null;
            _startTime = null;
            _endTime = null;
            _mainCatId = 0;
        }

        /// <summary>
        /// Gets the main reservation details from cart.
        /// </summary>
        /// <returns>The main reservation details from cart.<see cref="T:CGFSMVVM.Models.POSTDataModel"/></returns>
        public static POSTDataModel GetMainReservationDetailsFromCart()
        {
            var postModel = new POSTDataModel
            {
                GuestName = _guestName,
                GuestID = _guestID,
                RoomNo = _roomNum,
                ReservationNo = _resNum,
                GuestEmail = _guestEmail,
                GuestPhone = _guestPhone,
                GuestImage = _guestImage,
                ArrivalDate = _arrDate,
                DepartureDate = _depDate,
                Country = _country,
                CreatedBy = _createdBy,
                EndTime = _endTime,
                StartTime = _startTime,
                HtlCode = _hotelIdentifier,
                GuestAddress = _guestAddress,
                MainCategoryId = _mainCatId
            };

            return postModel;
        }
    }
}
