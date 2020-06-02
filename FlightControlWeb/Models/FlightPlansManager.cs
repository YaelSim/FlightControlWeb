using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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
        //Dictionary< uniqueId, KeyValuePair< isExternal, FlightPlan> >
        private readonly Dictionary<string, KeyValuePair<bool, FlightPlan>> flightPlans =
            new Dictionary<string, KeyValuePair<bool, FlightPlan>>();
        private readonly IServerManager serverManager;
        public FlightPlanManager(IServerManager sm)
        {
            serverManager = sm;
        }
        public async Task<IEnumerable<Flight>> GetAllFlightsRelative(DateTime dateTime)
        {
            List<Flight> allFlights = new List<Flight>();
            IEnumerable<Flight> internalFlights = GetInternalFlightsRelative(dateTime);
            allFlights.AddRange(internalFlights);

            string restOfUrl = "/api/Flights?relative_to=";
            //var externalServers = ((IEnumerable<Server>)cache.Get("serversList")).ToList();
            var externalServers = serverManager.GetAllServers();

            //For each server on the ServerManager
            foreach (Server currServer in externalServers)
            {
                IEnumerable<Flight> externalFlightsList = null;
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage returned;
                try
                {
                    returned = await httpClient.GetAsync(currServer.ServerURL
                            + restOfUrl + dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                } catch (HttpRequestException)
                {
                    continue;
                }
                //Make sure that the returned response was successful
                if (!returned.IsSuccessStatusCode)
                {
                    /*Controllers.HttpResponseException hre = new Controllers.HttpResponseException
                    {
                        Status = (int)returned.StatusCode,
                        Value = "External Server Response Unsuccessful"
                    };
                    throw hre;*/
                    continue;
                }

                string bodyOfReturned = await returned.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new FlightContractResolver()
                };
                externalFlightsList = JsonConvert.DeserializeObject<Flight[]>(
                    bodyOfReturned, settings);

                //Getting only flights that meet the concerns dictated.
                IEnumerable<Flight> properFlightsList = GetProperFlightsList(externalFlightsList);
                allFlights.AddRange(properFlightsList);

                await GetExternalServerFlightPlans(
                    externalFlightsList, httpClient, currServer.ServerURL);

                //Dispose the HttpClient to prevent a leak
                httpClient.Dispose();
            }
            
            return allFlights;
        }

        private IEnumerable<Flight> GetProperFlightsList(IEnumerable<Flight> externalFlightsList)
        {
            List<Flight> properFlightsList = new List<Flight>();
            foreach (Flight flight in externalFlightsList)
            {
                //Check if the given flight properties meet the concerns. If not - IGNORE IT.
                bool validFlight = CheckFlightProperties(flight);
                if (!validFlight)
                {
                    continue;
                }

                flight.IsExternal = true;
                properFlightsList.Add(flight);
            }
            return properFlightsList;
        }

        //Foreach Flight object on the flights list, get the matching flight plan.
        public async Task GetExternalServerFlightPlans(
            IEnumerable<Flight> flightsList, HttpClient httpClient, string serverUrl)
        {
            foreach (Flight flight in flightsList)
            {
                string id = flight.FlightId;
                HttpResponseMessage returned;
                try
                {
                    returned = await httpClient.GetAsync(serverUrl +
                        "/api/FlightPlan/" + id.ToString());
                } catch (HttpRequestException)
                {
                    continue;
                }
                //Make sure that the returned response was successful
                if (!returned.IsSuccessStatusCode)
                {
                    /*Controllers.HttpResponseException hre = new Controllers.HttpResponseException
                    {
                        Status = (int)returned.StatusCode,
                        Value = "External Server Response Unsuccessful"
                    };
                    throw hre;*/
                    continue;
                }

                string bodyOfReturned = await returned.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new FlightContractResolver()
                };
                FlightPlan flightPlan = JsonConvert.DeserializeObject<FlightPlan>(bodyOfReturned, settings);

                //Check if the given flightplan properties meet the concerns.
                bool valid = CheckFlightPlanProperties(flightPlan);
                if (!valid)
                {
                    continue;
                }

                //Add this flight plan to the dictionary.
                //It was already given a uniqueId (its' origin is from another server...)

                bool gotValue = flightPlans.TryGetValue(id, out _);

                if (gotValue)
                {
                    flightPlans.Remove(id);
                }
                flightPlans.Add(id, new KeyValuePair<bool, FlightPlan>(true, flightPlan));
            }
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
                DateTime dateTimeUTC = TimeZoneInfo.ConvertTimeToUtc(dateTime);

                //Manage only internal flights.
                if (isExternal)
                {
                    continue;
                }

                //add only if the flight is active
                if (IsFlightActive(flightPlan, dateTime))
                {
                    // get the updated location according to longitude and latitude
                    KeyValuePair<double, double> currentLocation = GetLocation(flightPlan,
                        dateTimeUTC);
                    flights.Add(new Flight
                    {
                        FlightId = flightId,
                        Longitude = currentLocation.Key,
                        Latitude = currentLocation.Value,
                        Passengers = flightPlan.Passengers,
                        CompanyName = flightPlan.CompanyName,
                        DateTime = flightPlan.InitialLocation.DateTime,
                        IsExternal = isExternal
                    });
                }
            }

            return flights;
        }
        public void AddFlightPlan(FlightPlan flightPlan)
        {
            string uniqueId = GenerateHashCodeOfId(flightPlan);
            bool isExternal = false;
            flightPlans.Add(uniqueId, new KeyValuePair<bool, FlightPlan>(isExternal, flightPlan));
        }
        public FlightPlan GetFlightPlanById(string uniqueId)
        {
            //Search within the local flight plans
            bool gotValue = flightPlans.TryGetValue(uniqueId, out KeyValuePair<bool,
                FlightPlan> output);

            if (gotValue)
            {
                return output.Value;
            }
            else
            {
                Debug.WriteLine("flightPlan isn't found.\n");
                return null;
            }
        }
        public FlightPlan RemoveFlightPlan(string uniqueId)
        {
            FlightPlan found = GetFlightPlanById(uniqueId);
            if (found == null)
            {
                return null;
            }
            flightPlans.Remove(uniqueId);
            return found;
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
            string id = (rand.Next(0, 9)).ToString();
            for (int i = 1; i <= 5; i++)
            {
                //get 6 random numbers
                id += (rand.Next(0, 9)).ToString();
            }
            return id;
        }
        public bool IsFlightActive(FlightPlan flightPlan, DateTime dateTime)
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
        public double GetTotalTimeOfFlight(List<Segment> segmentList)
        {
            double totalTime = 0;
            // combine all timespans of the flightplan's segments to create a total time
            foreach (Segment segment in segmentList)
            {
                totalTime += segment.TimespanSeconds;
            }
            return totalTime;
        }
        private KeyValuePair<double, double> GetLocation(FlightPlan flightPlan, DateTime dateTime)
        {
            // calculate the time that elapsed so far since the flight has begun
            TimeSpan elapsedSoFar = dateTime - flightPlan.InitialLocation.DateTime;
            double totalTime = elapsedSoFar.TotalSeconds;

            // get the current segment of this flight
            int currentSegmentIndex = GetFlightCurrentSegment(flightPlan.Segments, totalTime);

            //Determine the current initial location according to the currentSegmentIndex
            DateTime initialFlightTime = flightPlan.InitialLocation.DateTime;
            for (int i = 0; i < (currentSegmentIndex + 1); i++)
            {
                //initialFlightTime.AddSeconds(flightPlan.segments[i].Timespan_seconds);
                initialFlightTime = initialFlightTime.AddSeconds(flightPlan.Segments[i].TimespanSeconds);
            }

            //long ticksSoFar = dateTime.Ticks - initialFlightTime.Ticks;
            TimeSpan ticksSoFar = initialFlightTime - dateTime;
            double totalDistanceInSeconds = ticksSoFar.TotalSeconds;
            return CreateLocationAccordingToCurrentSegment(flightPlan, currentSegmentIndex,
                totalDistanceInSeconds);
        }

        //According to the given totalTime, determine in which segment the flight is currently located:
        //Everytime a segment whose timespan is smaller than totalTime is met, we reduce its' value.
        //When a segment whose timespan is smaller than (the current or updated) totalTime is met,
        // we'll return the current count value.
        public int GetFlightCurrentSegment(IEnumerable<Segment> segments, double totalTime)
        {
            int count = 0;
            foreach (Segment curr in segments)
            {
                //If the given totalTime > curr's timespan (measured in seconds)
                if (totalTime > curr.TimespanSeconds)
                {
                    totalTime -= curr.TimespanSeconds;
                    count += 1;
                    continue;
                }
                //Otherwise
                return count;
            }
            // in case of error
            return 0;
        }

        // After we've calculated the flight's current segment, determine where
        // exactly this flight is located, relatively to the current segment properties
        private KeyValuePair<double, double> CreateLocationAccordingToCurrentSegment(
            FlightPlan flightPlan, int currentSegmentIndex, double totalInSeconds)
        {
            Segment prevSegment, currentSegment;
            // If so, the flight's journey is yet to begin
            if (currentSegmentIndex == 0)
            {
                prevSegment = new Segment
                {
                    Longitude = flightPlan.InitialLocation.Longitude,
                    Latitude = flightPlan.InitialLocation.Latitude,
                    TimespanSeconds = 0
                };
            } else
            {
                prevSegment = flightPlan.Segments[currentSegmentIndex - 1];
            }

            // get the next segment loaction properties
            currentSegment = flightPlan.Segments[currentSegmentIndex];
            double distance = Math.Sqrt(Math.Pow(currentSegment.Longitude - prevSegment.Longitude,
                2) + Math.Pow(currentSegment.Latitude - currentSegment.Latitude, 2));
            if (distance == 0)
            {
                return new KeyValuePair<double, double>(currentSegment.Longitude,
                    currentSegment.Latitude);
            }
            double totalDistance = (totalInSeconds / currentSegment.TimespanSeconds) * distance;

            // Perform a linear interpolation in order to determine newXValue and newYValue
            double newXValue = currentSegment.Longitude - (totalDistance * (
                currentSegment.Longitude - prevSegment.Longitude) / distance);
            double newYValue = currentSegment.Latitude - (totalDistance * (
                currentSegment.Latitude - prevSegment.Latitude) / distance);

            // return the new values created
            return new KeyValuePair<double, double>(newXValue, newYValue);
        }

        //Given a flightplan, determine whether its properties meet the concerns
        public bool CheckFlightPlanProperties(FlightPlan flightPlan)
        {
            if (flightPlan == null)
            {
                return false;
            }
            if ((flightPlan.Passengers < 0) || (flightPlan.Passengers > Int32.MaxValue)) {
                return false;
            }
            if ((flightPlan.InitialLocation.Latitude > 90) || (
                flightPlan.InitialLocation.Latitude < -90)) {
                return false;
            }
            if ((flightPlan.InitialLocation.Longitude > 180) || (
                flightPlan.InitialLocation.Longitude < -180))
            {
                return false;
            }
            DateTime dt = flightPlan.InitialLocation.DateTime;
            if (dt == DateTime.MinValue)
            {
                return false;
            }
            if (!IsDateTimeWithinRange(dt))
            {
                return false;
            }
            if (flightPlan.CompanyName == null)
            {
                return false;
            }
            foreach (Segment segment in flightPlan.Segments)
            {
                if ((segment.Longitude > 180) || (segment.Longitude < -180))
                {
                    return false;
                }
                if ((segment.Latitude > 90) || (segment.Latitude < -90))
                {
                    return false;
                }
                if ((segment.TimespanSeconds < 0) || (segment.TimespanSeconds > Double.MaxValue))
                {
                    return false;
                }
            }
            return true;
        }

        //Given a flight, determine whether its properties meet the concerns
        private bool CheckFlightProperties(Flight flight)
        {
            if (flight == null)
            {
                return false;
            }
            if (flight.FlightId == null)
            {
                return false;
            }
            if ((flight.Passengers < 0) || (flight.Passengers > Int32.MaxValue))
            {
                return false;
            }
            if ((flight.Latitude > 90) || (flight.Latitude < -90))
            {
                return false;
            }
            if ((flight.Longitude > 180) || (flight.Longitude < -180))
            {
                return false;
            }
            if (flight.DateTime == DateTime.MinValue)
            {
                return false;
            }
            if (!IsDateTimeWithinRange(flight.DateTime))
            {
                return false;
            }
            if (flight.CompanyName == null)
            {
                return false;
            }
            return true;
        }

        //Given a dateTime, determine if it's within the possible range.
        public bool IsDateTimeWithinRange(DateTime toCheck)
        {
            //Make sure that toCheck is UTC.
            toCheck = TimeZoneInfo.ConvertTimeToUtc(toCheck);

            string lowerBoundStr = "0001-01-01T00:00:00Z";
            DateTime lowBound = DateTime.ParseExact(lowerBoundStr, "yyyy-MM-ddTHH:mm:ssZ",
                System.Globalization.CultureInfo.InvariantCulture);
            lowBound = TimeZoneInfo.ConvertTimeToUtc(lowBound);

            string upperBoundStr = "9999-12-31T11:59:59Z";
            DateTime upBound = DateTime.ParseExact(upperBoundStr, "yyyy-MM-ddTHH:mm:ssZ",
                System.Globalization.CultureInfo.InvariantCulture);
            upBound = TimeZoneInfo.ConvertTimeToUtc(upBound);

            bool comparison = toCheck >= lowBound && toCheck <= upBound;
            return comparison;
        }
    }
}