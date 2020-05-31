using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Server
    {
        [JsonProperty, JsonPropertyName("ServerId")]
        [Required]
        public string ServerId { get; set; }

        [JsonProperty, JsonPropertyName("ServerURL")]
        [Required]
        public string ServerURL { get; set; }
    }
}
