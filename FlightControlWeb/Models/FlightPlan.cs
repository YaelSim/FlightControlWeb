using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        [JsonProperty("passengers")]
        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "{0} value must be between {1} and {2}")]
        public int Passengers { get; set; }


        [JsonProperty("company_name")]
        [Required]
        public string CompanyName { get; set; }


        [JsonProperty("initial_location")]
        [Required]
        public InitialLocation InitialLocation {get; set; }

        [JsonProperty("segments")]
        [Required]
        public List<Segment> Segments { get; set; }
    }
}
