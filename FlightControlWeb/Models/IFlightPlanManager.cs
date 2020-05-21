using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightPlanManager
    {
        public Task<IEnumerable<Flight>> GetAllFlightsRelative(DateTime dateTime);
        public IEnumerable<Flight> GetInternalFlightsRelative(DateTime dateTime);
        public void AddFlightPlan(FlightPlan flightPlan);
        public FlightPlan GetFlightPlanById(string uniqueId);
        public void RemoveFlightPlan(string uniqueId);
    }
}
