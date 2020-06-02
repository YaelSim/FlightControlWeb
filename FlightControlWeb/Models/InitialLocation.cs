using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class InitialLocation
    {
        [JsonProperty, JsonPropertyName("longitude")]
        [Required]
        public double Longitude { get; set; } = -1000;


        [JsonProperty, JsonPropertyName("latitude")]
        [Required]
        public double Latitude { get; set; } = -1000;


        [JsonProperty, JsonPropertyName("date_time")]
        public DateTime DateTime { get; set; }
    }
}
