using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager
    {
        private readonly List<FlightPlan> flightsPlansList = new List<FlightPlan>();

        public IEnumerable<FlightPlan> GetAllFlightPlans()
        {
            return flightsPlansList;
        }

        public void AddFlightPlan(FlightPlan flightPlan)
        {
            flightsPlansList.Add(flightPlan);
        }

        public FlightPlan GetFlightPlanById(string uniqueId)
        {
            FlightPlan flightPlan = flightsPlansList.Where(x => x.UniqueId == uniqueId).FirstOrDefault();
            if (flightPlan == null)
            {
                Debug.WriteLine("flightPlan isn't found.\n");
                return null;
            }
            else
            {
                return flightPlan;
            }
        }

        public void RemoveFlightPlan(string uniqueId)
        {
            FlightPlan flightPlan = flightsPlansList.Where(x => x.UniqueId == uniqueId).FirstOrDefault();
            if (flightPlan == null)
            {
                Debug.WriteLine("flightPlan isn't found.\n");
            }
            else
            {
                flightsPlansList.Remove(flightPlan);
            }
        }
    }
}
