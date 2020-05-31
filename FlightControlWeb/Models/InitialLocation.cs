using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class InitialLocation
    {
        [JsonProperty("longitude")]
        [Required]
        [Range(-180.000001, 180, ErrorMessage = "{0} value must be between {1} and {2}")]
        public double Longitude { get; set; }


        [JsonProperty("latitude")]
        [Required]
        [Range(-90.000001, 90, ErrorMessage = "{0} value must be between {1} and {2}")]
        public double Latitude { get; set; }


        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
    }
}
