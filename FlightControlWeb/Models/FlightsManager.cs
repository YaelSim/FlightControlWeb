using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightsManager
    {
        private readonly List<Flight> flightsList = new List<Flight>();
        public IEnumerable<Flight> GetAllFlights()
        {
            return flightsList;
        }
        public void AddFlight(Flight flight)
        {
            flightsList.Add(flight);
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
            //oldFlight.InternalDateTime = ..... Create DataTime Object. Link:
            //https://stackoverflow.com/questions/11551185/converting-a-string-to-datetime-from-yyyy-mm-dd
            DateTime result = DateTime.ParseExact(flight.DateTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
        public void VerifyAllFlightsAreActive()
        {
            // TODO *********************************
            // If the flight isn't active anymore - remove it
        }
    }
}
