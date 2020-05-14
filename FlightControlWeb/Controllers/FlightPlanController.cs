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
        private FlightPlanManager service = new FlightPlanManager();
        
        //POST: /api/FlightPlan
        [HttpPost]
        public FlightPlan AddFlightPlan(FlightPlan fp)
        {
            service.AddFlightPlan(fp);
            return fp;
        }
        //GET: /api/FlightPlan/{id}
        [HttpGet]
        public FlightPlan GetFlightPlanByUniqueId(string uniqueId)
        {
            return service.GetFlightPlanById(uniqueId);
        }
    }
}