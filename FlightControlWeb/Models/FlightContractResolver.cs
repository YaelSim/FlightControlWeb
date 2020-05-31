using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace FlightControlWeb.Models
{
    public class FlightContractResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        //run time mapping between json and flightplan,flight and server objects.
        public FlightContractResolver()
        {
            this.PropertyMappings = new Dictionary<string, string>
            {
                {"DateTime", "date_time"},
                {"CompanyName", "company_name"},
                {"Latitude", "latitude"},
                {"FlightId", "flight_id"},
                {"Longitude", "longitude"},
                {"Passengers", "passengers"},
                {"IsExternal", "is_external"},
                {"ServerId" ,"ServerId"},
                {"ServerURL" ,"ServerURL"},
                {"Segments","segments" },
                {"InitialLocation","initial_location" },
                {"TimespanSeconds","timespan_seconds" }
            };
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out string resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}