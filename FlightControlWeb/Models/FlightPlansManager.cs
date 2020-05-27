using Microsoft.AspNetCore.Mvc;
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

            //********************************************
            string example = "2020-05-27T07:00:47Z";
            DateTime res = DateTime.ParseExact(example, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            res = TimeZoneInfo.ConvertTimeToUtc(res);
            List<Segment> segmentsRealmo = new List<Segment>
            {
                new Segment
                {
                    longitude = 32,
                    latitude = 62,
                    timespan_seconds = 1160
                },
                new Segment
                {
                    longitude = 13,
                    latitude = 7,
                    timespan_seconds = 9560
                }
            };
            List<Segment> segmentsISRAIR = new List<Segment>
            {
                new Segment
                {
                    longitude = 35.211514,
                    latitude = 31.769399,
                    timespan_seconds = 10000
                },
                new Segment
                {
                    longitude = 34.781668,
                    latitude = 32.113742,
                    timespan_seconds = 60000
                }
            };
            List<Segment> segmentsAquoavo = new List<Segment>
            {
                new Segment
                {
                    longitude = 177,
                    latitude = 88,
                    timespan_seconds = 5670
                },
                new Segment
                {
                    longitude = 180,
                    latitude = 85,
                    timespan_seconds = 4200
                }
            };
            List<Segment> segmentsDigigenAirlines = new List<Segment>
            {
                new Segment
                {
                    longitude = 48,
                    latitude = 71,
                    timespan_seconds = 9170
                },
                new Segment
                {
                    longitude = 150,
                    latitude = 48,
                    timespan_seconds = 4930
                }
            };
            List<Segment> segmentsEmtrakAirways = new List<Segment>
            {
                new Segment
                {
                    longitude = 13,
                    latitude = 84,
                    timespan_seconds = 3030
                },
                new Segment
                {
                    longitude = 10,
                    latitude = 13,
                    timespan_seconds = 4900
                },
                new Segment
                {
                    longitude = 34,
                    latitude = 22,
                    timespan_seconds = 6750
                },
                new Segment
                {
                    longitude = 31,
                    latitude = 76,
                    timespan_seconds = 3090
                },
            };
            List<Segment> segments = new List<Segment>
            {
                new Segment
                {
                    longitude = 4,
                    latitude = 4,
                    timespan_seconds = 1000
                },
                new Segment
                {
                    longitude = 5,
                    latitude = 5,
                    timespan_seconds = 1500
                },
                new Segment
                {
                    longitude = 6,
                    latitude = 6,
                    timespan_seconds = 2000
                }
            };

            this.AddFlightPlan(new FlightPlan
            {
                CompanyName = "1",
                InitialLocation = new InitialLocation
                {
                    longitude = 1,
                    latitude = 1,
                    date_time = res
                },
                Segments = segments
            });
            this.AddFlightPlan(new FlightPlan
            {
                Passengers = 236,
                CompanyName = "Realmo Air",
                InitialLocation = new InitialLocation
                {
                    latitude = 4,
                    longitude = 120,
                    date_time = res
                },
                Segments = segmentsRealmo
            });
            this.AddFlightPlan(new FlightPlan
            {
                Passengers = 236,
                CompanyName = "ISRAIR",
                InitialLocation = new InitialLocation
                {
                    latitude = 29.555631,
                    longitude = 34.957610,
                    date_time = res
                },
                Segments = segmentsISRAIR
            });
            this.AddFlightPlan(new FlightPlan
            {
                Passengers = 311,
                CompanyName = "Aquoavo",
                InitialLocation = new InitialLocation
                {
                    latitude = 76,
                    longitude = 89,
                    date_time = res
                },
                Segments = segmentsAquoavo
            });
            this.AddFlightPlan(new FlightPlan
            {
                Passengers = 242,
                CompanyName = "Digigen Airlines",
                InitialLocation = new InitialLocation
                {
                    latitude = 35,
                    longitude = 50,
                    date_time = res
                },
                Segments = segmentsDigigenAirlines
            });
            this.AddFlightPlan(new FlightPlan
            {
                CompanyName = "2",
                InitialLocation = new InitialLocation
                {
                    longitude = 2,
                    latitude = 2,
                    date_time = res
                },
                Segments = segments
            });
            this.AddFlightPlan(new FlightPlan
            {
                Passengers = 236,
                CompanyName = "Emtrak Airways",
                InitialLocation = new InitialLocation
                {
                    latitude = 34,
                    longitude = 158,
                    date_time = res
                },
                Segments = segmentsEmtrakAirways
            });
            this.AddFlightPlan(new FlightPlan
            {
                CompanyName = "3",
                InitialLocation = new InitialLocation
                {
                    longitude = 3,
                    latitude = 3,
                    date_time = res
                },
                Segments = segments
            });
            //*************************************
        }
        public async Task<IEnumerable<Flight>> GetAllFlightsRelative(DateTime dateTime)
        {
            List<Flight> allFlights = new List<Flight>();
            IEnumerable<Flight> internalFlights = GetInternalFlightsRelative(dateTime);
            allFlights.AddRange(internalFlights);

            string restOfUrl = "/api/Flights?relative_to=";
            IEnumerable<Server> externalServers = serverManager.GetAllServers();

            //For each server on the ServerManager
            foreach (Server currServer in externalServers)
            {
                IEnumerable<Flight> externalFlightsList = null;
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage returned = await httpClient.GetAsync(currServer.ServerURL
                        + restOfUrl + dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

                //Make sure that the returned response was successful
                //returned.EnsureSuccessStatusCode();

                if (!returned.IsSuccessStatusCode)
                {
                    Controllers.HttpResponseException hre = new Controllers.HttpResponseException
                    {
                        //StatusCode = returned.StatusCode,
                        Status = (int)returned.StatusCode,
                        Value = "External Server Response Unsuccessful"
                    };
                    throw hre;
                }

                string bodyOfReturned = await returned.Content.ReadAsStringAsync();
                externalFlightsList = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<IEnumerable<Flight>>(bodyOfReturned);

                foreach (Flight curr in externalFlightsList) {
                    curr.IsExternal = true;
                    allFlights.Add(curr);
                }

                GetExternalServerFlightPlans(externalFlightsList, httpClient, currServer.ServerURL);

                //Dispose the HttpClient to prevent a leak
                httpClient.Dispose();
            }
            
            return allFlights;
        }

        //Maybe this method should return  Task<IEnumerable<FlightPlan>> ??? ***************
        //Foreach Flight object on the flights list, get the matching flight plan.
        public async void GetExternalServerFlightPlans(
            IEnumerable<Flight> flightsList, HttpClient httpClient, string serverUrl)
        {
            foreach (Flight flight in flightsList)
            {
                string id = flight.FlightId;
                HttpResponseMessage returned = await httpClient.GetAsync(serverUrl + 
                    "/api/FlightPlan/" + id);
                string bodyOfReturned = await returned.Content.ReadAsStringAsync();
                FlightPlan flightPlan = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPlan>(
                    bodyOfReturned);

                //Add this flight plan to the dictionary.
                //It was already given a uniqueId (its' origin is from another server...)
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
                        DateTime = flightPlan.InitialLocation.date_time,
                        IsExternal = isExternal
                    });
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
            //Search within the local flight plans
            bool gotValue = flightPlans.TryGetValue(uniqueId, out KeyValuePair<bool,
                FlightPlan> output);

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
        private bool IsFlightActive(FlightPlan flightPlan, DateTime dateTime)
        {
            dateTime = dateTime.ToUniversalTime();
            DateTime initTime = flightPlan.InitialLocation.date_time.ToUniversalTime();
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
                totalTime += segment.timespan_seconds;
            }
            return totalTime;
        }
        private KeyValuePair<double, double> GetLocation(FlightPlan flightPlan, DateTime dateTime)
        {
            // calculate the time that elapsed so far since the flight has begun
            TimeSpan elapsedSoFar = dateTime - flightPlan.InitialLocation.date_time;
            double totalTime = elapsedSoFar.TotalSeconds;

            // get the current segment of this flight
            int currentSegmentIndex = GetFlightCurrentSegment(flightPlan.Segments, totalTime);

            //Determine the current initial location according to the currentSegmentIndex
            DateTime initialFlightTime = flightPlan.InitialLocation.date_time;
            for (int i = 0; i < (currentSegmentIndex + 1); i++)
            {
                //initialFlightTime.AddSeconds(flightPlan.segments[i].Timespan_seconds);
                initialFlightTime = initialFlightTime.AddSeconds(flightPlan.Segments[i].timespan_seconds);
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
        private int GetFlightCurrentSegment(IEnumerable<Segment> segments, double totalTime)
        {
            int count = 0;
            foreach (Segment curr in segments)
            {
                //If the given totalTime > curr's timespan (measured in seconds)
                if (totalTime > curr.timespan_seconds)
                {
                    totalTime = totalTime - curr.timespan_seconds;
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
                    longitude = flightPlan.InitialLocation.longitude,
                    latitude = flightPlan.InitialLocation.latitude,
                    timespan_seconds = 0
                };
            } else
            {
                prevSegment = flightPlan.Segments[currentSegmentIndex - 1];
            }

            // get the next segment loaction properties
            currentSegment = flightPlan.Segments[currentSegmentIndex];
            double distance = Math.Sqrt(Math.Pow(currentSegment.longitude - prevSegment.longitude,
                2) + Math.Pow(currentSegment.latitude - currentSegment.latitude, 2));
            double totalDistance = (totalInSeconds / currentSegment.timespan_seconds) * distance;

            // Perform a linear interpolation in order to determine newXValue and newYValue
            double newXValue = currentSegment.longitude - (totalDistance * (
                currentSegment.longitude - prevSegment.longitude) / distance);
            double newYValue = currentSegment.latitude - (totalDistance * (
                currentSegment.latitude - prevSegment.latitude) / distance);

            // return the new values created
            return new KeyValuePair<double, double>(newXValue, newYValue);
        }
    }
}