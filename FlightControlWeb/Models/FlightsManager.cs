using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    /*public class FlightsManager
    {
        private readonly List<Flight> flightsList = new List<Flight>();
        
        //Return internal AND external flights according to a given <DATE_TIME>
        public IEnumerable<Flight> GetAllFlights()
        {
            return flightsList;
        }

        //Return internal flights only according to a given <DATE_TIME>
        public IEnumerable<Flight> GetAllInternalFlights()
        {
            List<Flight> internalFlightsList = new List<Flight>();
            foreach (var flight in flightsList)
            {
                if (!flight.IsExternal)
                {
                    internalFlightsList.Add(flight);
                }
            }
            return internalFlightsList;
        }
        public void AddFlight(Flight flight)
        {
            Flight toAdd = flight;
            //DateTime result = DateTime.ParseExact(flight.DateTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            //result = TimeZoneInfo.ConvertTimeToUtc(result);
           // toAdd.InternalDateTime = result;
            flightsList.Add(toAdd);
        }
        public void DeleteFlight(string flightId)
        {
            Flight flight = flightsList.Where(x => x.FlightId == flightId).FirstOrDefault();
            if (flight == null)
            {
                Debug.WriteLine("flight isn't found.\n");
            }
            else
            {
                flightsList.Remove(flight);
            }
        }
        public Flight GetFlightById(string id)
        {
            Flight flight = flightsList.Where(x => x.FlightId == id).FirstOrDefault();
            if (flight == null)
            {
                Debug.WriteLine("flight isn't found.\n");
                return null;
            }
            else
            {
                return flight;
            }
        }
        public void UpdateFlight(Flight flight)
        {
            Flight oldFlight = flightsList.Where(x => x.FlightId == flight.FlightId).FirstOrDefault();
            oldFlight.CompanyName = flight.CompanyName;
            oldFlight.DateTime = flight.DateTime;
            oldFlight.IsExternal = flight.IsExternal;
            oldFlight.Latitude = flight.Latitude;
            oldFlight.Longitude = flight.Longitude;
            oldFlight.Passengers = flight.Passengers;
            
            //Convert the given string to an DateTime object
            DateTime result = DateTime.ParseExact(flight.DateTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            result = TimeZoneInfo.ConvertTimeToUtc(result);
            oldFlight.InternalDateTime = result;
        }
    }*/
}
