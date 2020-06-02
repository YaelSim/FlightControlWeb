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
    public class FlightPlanController : ControllerBase
    {
        private readonly IFlightPlanManager service;

        // Constructor
        public FlightPlanController(IFlightPlanManager flightPlanManager)
        {
            service = flightPlanManager;
        }

        //POST: /api/FlightPlan
        [HttpPost]
        public FlightPlan AddFlightPlan([FromBody] FlightPlan fp)
        {
            // Check if the given flightplan properties meet the concerns.
            bool valid = service.CheckFlightPlanProperties(fp);
            if (!valid)
            {
                HttpResponseException hre = new HttpResponseException
                {
                    Status = 400,
                    Value = "FlightPlan Cannot Be Added - Metadata Doesn't Meet Concerns."
                };
                throw hre;
            }
            service.AddFlightPlan(fp);
            return fp;
        }

        //GET: /api/FlightPlan/{id}
        [HttpGet("{id}")]
        public FlightPlan GetFlightPlanByUniqueId(string id)
        {
            FlightPlan found = service.GetFlightPlanById(id);
            if (found == null)
            {
                HttpResponseException hre = new HttpResponseException
                {
                    Status = 404,
                    Value = "FlightPlan Was Not Found"
                };
                throw hre;
            } else
            {
                return found;
            }
        }
    }
}