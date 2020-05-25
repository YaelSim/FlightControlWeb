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
        public FlightPlanController(IFlightPlanManager flightPlanManager)
        {
            this.service = flightPlanManager;
            this.AddFlightPlan(new FlightPlan { CompanyName = "1", InitialLocation = new InitialLocation { Longitude = 1, Latitude = 1, DateTime = new DateTime() }, Segments = null });
            this.AddFlightPlan(new FlightPlan { CompanyName = "2", InitialLocation = new InitialLocation { Longitude = 2, Latitude = 2, DateTime = new DateTime() }, Segments = null });
            this.AddFlightPlan(new FlightPlan { CompanyName = "3", InitialLocation = new InitialLocation { Longitude = 3, Latitude = 3, DateTime = new DateTime() }, Segments = null });


        }

        //POST: /api/FlightPlan
        [HttpPost]
        public FlightPlan AddFlightPlan([FromBody] FlightPlan fp)
        {
            service.AddFlightPlan(fp);
            return fp;
        }

        //GET: /api/FlightPlan/{id}
        [HttpGet("{id}")]
        public FlightPlan GetFlightPlanByUniqueId(string uniqueId)
        {
            return service.GetFlightPlanById(uniqueId);
        }
    }
}