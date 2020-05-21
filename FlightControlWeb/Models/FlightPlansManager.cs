using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager : IFlightPlanManager
    {
        private readonly Dictionary<string, KeyValuePair<bool, FlightPlan>> flightPlans =
            new Dictionary<string, KeyValuePair<bool, FlightPlan>>();
        private readonly IServerManager serverManager;
        public FlightPlanManager(IServerManager sm)
        {
            serverManager = sm;
        }
        public async Task<IEnumerable<Flight>> GetAllFlightsRelative(DateTime dateTime)
        {
            //HERE the previous code was......
            IEnumerable<Flight> flightsTotal = GetInternalFlightsRelative(dateTime);
            string restOfUrl = "/api/Flights?relative_to=";
            //For each server on the ServerManager
            foreach (Server currServer in serverManager.GetAllServers())
            {
                IEnumerable<Flight> flightsList = null;
                HttpClient httpClient = new HttpClient();
                try
                {
                    HttpResponseMessage returned = await httpClient.GetAsync(currServer.ServerUrl 
                        + restOfUrl + dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

                    //Make sure that the returned response was successful
                    returned.EnsureSuccessStatusCode();
                    string bodyOfReturned = await returned.Content.ReadAsStringAsync();
                    Debug.WriteLine(bodyOfReturned); //DELETE AFTERWARDS *************
                    flightsList = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Flight>>(bodyOfReturned);
                } catch (Exception e)
                {
                    Debug.WriteLine("\nException Caught...\n {0}", e.Message);
                }

                //Dispose the HttpClient to prevent a leak
                httpClient.Dispose();

                //Combine the flightsList to flightsTotal. DO WE NEED TO PROVIDE A COMPARATOR? NOT SURE. *************
                flightsTotal.Union(flightsList);
            }
            
            return flightsTotal;
        }
        public IEnumerable<Flight> GetInternalFlightsRelative(DateTime dateTime)
        {
            List<Flight> flights = new List<Flight>();
            foreach (KeyValuePair<string, KeyValuePair<bool, FlightPlan>> flightPlanKeyValuePair
                in this.flightPlans)
            {
                FlightPlan flightPlan = flightPlanKeyValuePair.Value.Value;
                string flightId = flightPlanKeyValuePair.Key;
                bool isExternal = flightPlanKeyValuePair.Value.Key;

                if (!isExternal)
                {
                    //add only if the flight is active
                    if (IsFlightActive(flightPlan, dateTime))
                    {
                        flights.Add(new Flight
                        {
                            FlightId = flightId,
                            Longitude = flightPlan.InitialLocation.Longitude,
                            Latitude = flightPlan.InitialLocation.Latitude,
                            Passengers = flightPlan.Passengers,
                            CompanyName = flightPlan.CompanyName,
                            DateTime = flightPlan.InitialLocation.DateTime,
                            IsExternal = isExternal
                        });
                    }
                }
            }

            return flights;
        }
        public void AddFlightPlan(FlightPlan flightPlan)
        {
            string uniqueId = this.GenerateHashCodeOfId(flightPlan);
            bool isExternal = false;
            flightPlans.Add(uniqueId, new KeyValuePair<bool, FlightPlan>(isExternal, flightPlan));
        }
        public FlightPlan GetFlightPlanById(string uniqueId)
        {
            bool gotValue = flightPlans.TryGetValue(uniqueId, out KeyValuePair<bool, FlightPlan> output);

            if (gotValue)
            {
                return output.Value;
            }
            else
            {
                //throw new Exception("No flight found");
                Debug.WriteLine("flightPlan isn't found.\n");
                return null;
            }
        }
        public void RemoveFlightPlan(string uniqueId)
        {
            this.flightPlans.Remove(uniqueId);
        }
        private string GenerateHashCodeOfId(FlightPlan flightPlan)
        {
            string firstName;
            //If the given flightplan's parameters are too short to generate
            if (flightPlan.CompanyName.Length < 2)
            {
                firstName = flightPlan.CompanyName;
            } else
            {
                // get the first 2 chars from the company name
                firstName = (flightPlan.CompanyName).Substring(0, 2);
            }
            
            // id - first 2 char and 6 random numbers
            string Id = firstName + GetRandomNumbers();
            return Id;
        }
        private string GetRandomNumbers()
        {
            Random rand = new Random();
            string id = (rand.Next()).ToString();
            for (int i = 1; i <= 5; i++)
            {
                //get 6 random numbers
                id += (rand.Next()).ToString();
            }
            return id;
        }
        private bool IsFlightActive(FlightPlan flightPlan, DateTime dateTime)
        {
            dateTime = dateTime.ToUniversalTime();
            DateTime initTime = flightPlan.InitialLocation.DateTime.ToUniversalTime();
            int result = DateTime.Compare(dateTime, initTime);
            // the flight is not active yet 
            if (result < 0)
            {
                return false;
            }

            double totalFlightTime = GetTotalTimeOfFlight(flightPlan.Segments);
            DateTime endFlight = (initTime.AddSeconds(totalFlightTime)).ToUniversalTime();
            result = DateTime.Compare(dateTime, endFlight);
            // if its later then the flight is not active
            if (result > 0)
            {
                return false;
            }
            return true;
        }
        private double GetTotalTimeOfFlight(List<Segment> segmentList)
        {
            double totalTime = 0;
            // combine all timespans of the flightplan's segments to create a total time
            foreach (Segment segment in segmentList)
            {
                totalTime += segment.Timespan_seconds;
            }
            return totalTime;
        }
    }
}