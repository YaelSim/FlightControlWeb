using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager
    {
        //private readonly List<FlightPlan> flightsPlansList = new List<FlightPlan>(); * old
        readonly Dictionary<string, KeyValuePair<bool, FlightPlan>> flightPlans =
            new Dictionary<string, KeyValuePair<bool, FlightPlan>>();

        public IEnumerable<Flight> GetAllFlightsRelative(DateTime dateTime)
        {
            List<Flight> flights = new List<Flight>();
            foreach (KeyValuePair<string, KeyValuePair<bool, FlightPlan>> flightPlanKeyValuePair
                in this.flightPlans)
            {
                FlightPlan flightPlan = flightPlanKeyValuePair.Value.Value;
                string flightId = flightPlanKeyValuePair.Key;
                bool isExternal = flightPlanKeyValuePair.Value.Key;

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

            return flights;
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
            KeyValuePair<bool, FlightPlan> output;
            bool gotValue = flightPlans.TryGetValue(uniqueId, out output);

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
            // get the first 2 chars from the company name
            string firstName = (flightPlan.CompanyName).Substring(0, 2);
            // id - first 2 char and 6 random numbers
            string Id = firstName + getRandomNumbers();
            return Id;
        }

        private string getRandomNumbers()
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