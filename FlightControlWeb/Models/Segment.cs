using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Segment
    {
        [JsonProperty, JsonPropertyName("longitude")]
        [Required]
        [Range(-180.000001, 180, ErrorMessage = "{0} value must be between {1} and {2}")]
        public double Longitude { get; set; }


        [JsonProperty, JsonPropertyName("latitude")]
        [Required]
        [Range(-90.000001, 90, ErrorMessage = "{0} value must be between {1} and {2}")]
        public double Latitude { get; set; }


        [JsonProperty, JsonPropertyName("timespan_seconds")]
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "{0} value must be between {1} and {2}")]
        public double TimespanSeconds { get; set; }
    }
}
