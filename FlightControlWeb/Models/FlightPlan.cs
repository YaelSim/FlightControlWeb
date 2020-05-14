using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        public string UniqueId
        {
            get { return UniqueId; }
            set
            {
                // TODO Implement has function!
            }
        }
        public int Passengers { get; set; }
        public string CompanyName { get; set; }
        public InitialLocation InitialLocation {get; set; }
        public List<Segment> Segments { get; set; }
    }
}
