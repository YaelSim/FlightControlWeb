using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        [JsonProperty, JsonPropertyName("passengers")]
        [Required]
        public int Passengers { get; set; }


        [JsonProperty, JsonPropertyName("company_name")]
        [Required]
        public string CompanyName { get; set; }


        [JsonProperty, JsonPropertyName("initial_location")]
        [Required]
        public InitialLocation InitialLocation {get; set; }

        [JsonProperty, JsonPropertyName("segments")]
        [Required]
        public List<Segment> Segments { get; set; }
    }
}
