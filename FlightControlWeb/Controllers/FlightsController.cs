using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private FlightsManager service = new FlightsManager();

        // GET: /api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights()
        {
            return service.GetAllFlights();
        }

        // GET: /api/Flights?relative_to=<DATE_TIME>&sync_all
        [HttpGet]


        // DELETE: /api/Flights/{id}
        [HttpDelete]
        public void DeleteFlights(string id)
        {
            service.DeleteFlight(id);
            // check if need to delete the flightPlan*******

        }
    }
}