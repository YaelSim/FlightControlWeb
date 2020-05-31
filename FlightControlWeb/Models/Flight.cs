using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Flight
    {
        [JsonProperty, JsonPropertyName("flight_id")]
        [Required]
        public string FlightId { get; set; }


        [JsonProperty, JsonPropertyName("longitude")]
        [Required]
        [Range(-180.000001, 180, ErrorMessage = "{0} value must be between {1} and {2}")]
        public double Longitude { get; set; }


        [JsonProperty, JsonPropertyName("latitude")]
        [Required]
        [Range(-90.000001, 90, ErrorMessage = "{0} value must be between {1} and {2}")]
        public double Latitude { get; set; }


        [JsonProperty, JsonPropertyName("passengers")]
        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "{0} value must be between {1} and {2}")]
        public int Passengers { get; set; }


        [JsonProperty, JsonPropertyName("company_name")]
        [Required]
        public string CompanyName { get; set; }


        [JsonProperty, JsonPropertyName("date_time")]
        [Required]
        public DateTime DateTime { get; set; }


        [JsonProperty, JsonPropertyName("is_external")]
        [Required]
        public bool IsExternal { get; set; }
    }
}
