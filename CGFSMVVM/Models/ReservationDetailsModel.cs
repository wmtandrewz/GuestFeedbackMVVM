using System;
using Newtonsoft.Json;

namespace CGFSMVVM.Models
{
    public class ReservationDetailsModel
    {
        [JsonProperty("GuestId")]
        public string GuestId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Nationality")]
        public string Nationality { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Telephone")]
        public string Telephone { get; set; }

        [JsonProperty("Mobile")]
        public string Mobile { get; set; }

        [JsonProperty("RoomNo")]
        public string RoomNo { get; set; }

        [JsonProperty("BookingId")]
        public string BookingId { get; set; }

        [JsonProperty("ArrivalDate")]
        public string ArrivalDate { get; set; }

        [JsonProperty("DepartureDate")]
        public string DepartureDate { get; set; }
    
    }
}
