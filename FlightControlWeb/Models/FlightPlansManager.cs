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

            //********************************************
            string example = "2020-05-21T16:32:22Z";
            DateTime res = DateTime.ParseExact(example, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            res = TimeZoneInfo.ConvertTimeToUtc(res);
            List<Segment> segmentsRealmo = new List<Segment>
            {
                new Segment
                {
                    longitude = 32,
                    latitude = 62,
                    timespan_seconds = 116
                },
                new Segment
                {
                    longitude = 13,
                    latitude = 7,
                    timespan_seconds = 956
                }
            };
            List<Segment> segmentsAquoavo = new List<Segment>
            {
                new Segment
                {
                    longitude = 177,
                    latitude = 88,
                    timespan_seconds = 567
                },
                new Segment
                {
                    longitude = 180,
                    latitude = 85,
                    timespan_seconds = 420
                }
            };
            List<Segment> segmentsDigigenAirlines = new List<Segment>
            {
                new Segment
                {
                    longitude = 48,
                    latitude = 71,
                    timespan_seconds = 917
                },
                new Segment
                {
                    longitude = 150,
                    latitude = 48,
                    timespan_seconds = 493
                }
            };
            List<Segment> segmentsEmtrakAirways = new List<Segment>
            {
                new Segment
                {
                    longitude = 13,
                    latitude = 84,
                    timespan_seconds = 303
                },
                new Segment
                {
                    longitude = 10,
                    latitude = 13,
                    timespan_seconds = 490
                },
                new Segment
                {
                    longitude = 34,
                    latitude = 22,
                    timespan_seconds = 675
                },
                new Segment
                {
                    longitude = 31,
                    latitude = 76,
                    timespan_seconds = 309
                },
            };
            List<Segment> segments = new List<Segment>
            {
                new Segment
                {
                    longitude = 4,
                    latitude = 4,
                    timespan_seconds = 10
                },
                new Segment
                {
                    longitude = 5,
                    latitude = 5,
                    timespan_seconds = 15
                },
                new Segment
                {
                    longitude = 6,
                    latitude = 6,
                    timespan_seconds = 20
                }
            };

            this.AddFlightPlan(new FlightPlan
            {
                company_name = "1",
                initial_location = new InitialLocation
                {
                    longitude = 1,
                    latitude = 1,
                    date_time = res
                },
                segments = segments
            });
            this.AddFlightPlan(new FlightPlan
            {
                passengers = 236,
                company_name = "Realmo Air",
                initial_location = new InitialLocation
                {
                    latitude = 4,
                    longitude = 120,
                    date_time = res
                },
                segments = segmentsRealmo
            });
            this.AddFlightPlan(new FlightPlan
            {
                passengers = 311,
                company_name = "Aquoavo",
                initial_location = new InitialLocation
                {
                    latitude = 76,
                    longitude = 89,
                    date_time = res
                },
                segments = segmentsAquoavo
            });
            this.AddFlightPlan(new FlightPlan
            {
                passengers = 242,
                company_name = "Digigen Airlines",
                initial_location = new InitialLocation
                {
                    latitude = 35,
                    longitude = 50,
                    date_time = res
                },
                segments = segmentsDigigenAirlines
            });
            this.AddFlightPlan(new FlightPlan
            {
                company_name = "2",
                initial_location = new InitialLocation
                {
                    longitude = 2,
                    latitude = 2,
                    date_time = res
                },
                segments = segments
            });
            this.AddFlightPlan(new FlightPlan
            {
                passengers = 236,
                company_name = "Emtrak Airways",
                initial_location = new InitialLocation
                {
                    latitude = 34,
                    longitude = 158,
                    date_time = res
                },
                segments = segmentsEmtrakAirways
            });
            this.AddFlightPlan(new FlightPlan
            {
                company_name = "3",
                initial_location = new InitialLocation
                {
                    longitude = 3,
                    latitude = 3,
                    date_time = res
                },
                segments = segments
            });
            //*************************************
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
                    HttpResponseMessage returned = await httpClient.GetAsync(currServer.ServerURL 
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
                        // get the updated location according to longitude and latitude
                        KeyValuePair<double, double> currentLocation = GetLocation(flightPlan,
                            dateTime);
                        flights.Add(new Flight
                        {
                            flight_id = flightId,
                            longitude = currentLocation.Key,
                            latitude = currentLocation.Value,
                            passengers = flightPlan.passengers,
                            company_name = flightPlan.company_name,
                            date_time = flightPlan.initial_location.date_time, //***??? is initial???***
                            is_external = isExternal
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
            if (flightPlan.company_name.Length < 2)
            {
                firstName = flightPlan.company_name;
            } else
            {
                // get the first 2 chars from the company name
                firstName = (flightPlan.company_name).Substring(0, 2);
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
            DateTime initTime = flightPlan.initial_location.date_time.ToUniversalTime();
            int result = DateTime.Compare(dateTime, initTime);
            // the flight is not active yet 
            if (result < 0)
            {
                return false;
            }

            double totalFlightTime = GetTotalTimeOfFlight(flightPlan.segments);
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
            // calculate the time that elapsed so far since the flight has begun (dateTime.ticks)
            long elapsedSoFar = dateTime.Ticks - flightPlan.initial_location.date_time.Ticks;
            double totalTime = TimeSpan.FromTicks(elapsedSoFar).TotalSeconds;

            // get the current segment of this flight
            int currentSegmentIndex = GetFlightCurrentSegment(flightPlan.segments, totalTime);

            //Determine the current initial location according to the currentSegmentIndex
            DateTime initialFlightTime = flightPlan.initial_location.date_time;
            for (int i = 0; i < (currentSegmentIndex + 1); i++)
            {
                //initialFlightTime.AddSeconds(flightPlan.segments[i].Timespan_seconds);
                initialFlightTime = initialFlightTime.AddSeconds(flightPlan.segments[i].timespan_seconds);
            }

            //long ticksSoFar = dateTime.Ticks - initialFlightTime.Ticks;
            long ticksSoFar = Math.Abs( dateTime.Ticks - initialFlightTime.Ticks );
            double totalDistanceInSeconds = TimeSpan.FromTicks(ticksSoFar).TotalSeconds;
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
            FlightPlan flightPlan, int currentSegmentIndex, double totalDistance)
        {
            Segment prevSegment, currentSegment;
            // If so, the flight's journey is yet to begin
            if (currentSegmentIndex == 0)
            {
                prevSegment = new Segment
                {
                    longitude = flightPlan.initial_location.longitude,
                    latitude = flightPlan.initial_location.latitude,
                    timespan_seconds = 0
                };
            } else
            {
                prevSegment = flightPlan.segments[currentSegmentIndex - 1];
            }

            // get the next segment loaction properties
            currentSegment = flightPlan.segments[currentSegmentIndex];
            double relativeTime = totalDistance / (currentSegment.timespan_seconds);

            // Perform a linear interpolation in order to determine newXValue and newYValue
            double newXValue = prevSegment.longitude + ((prevSegment.latitude -
                prevSegment.longitude) / relativeTime);
            double newYValue = currentSegment.longitude + ((currentSegment.latitude -
                currentSegment.longitude) / relativeTime);

            // return the new values created
            return new KeyValuePair<double, double>(newXValue, newYValue);
        }
    }
}