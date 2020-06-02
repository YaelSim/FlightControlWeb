﻿using Newtonsoft.Json;
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
        public double Longitude { get; set; }


        [JsonProperty, JsonPropertyName("latitude")]
        [Required]
        public double Latitude { get; set; }


        [JsonProperty, JsonPropertyName("timespan_seconds")]
        [Required]
        public double TimespanSeconds { get; set; }
    }
}
