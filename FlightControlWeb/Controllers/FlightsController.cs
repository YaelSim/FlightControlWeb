using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;
using System.Globalization;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private IFlightPlanManager service;
        public FlightsController(IFlightPlanManager flightPlanManager)
        {
            this.service = flightPlanManager;
        }

        // GET: /api/Flights?relative_to=<DATE_TIME>&sync_all   V   GET: /api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public async Task<IEnumerable<Flight>> GetFlights(
            [FromQuery(Name = "relative_to")] string dateTime)
        {
            string request = Request.QueryString.Value;
            DateTime result = GetDateTimeAccordingfToStr(dateTime);
            bool externalFlightsNeeded = request.Contains("sync_all");
            if (externalFlightsNeeded)
            {
                return await service.GetAllFlightsRelative(result);
            } else
            {
                return service.GetInternalFlightsRelative(result);
            }
        }

        // DELETE: /api/Flights/{id}
        [HttpDelete("{id}")]
        public void DeleteFlightPlan(string id)
        {
            service.RemoveFlightPlan(id);
        }

        private DateTime GetDateTimeAccordingfToStr(string dateTime)
        {
            DateTime result = DateTime.ParseExact(dateTime, "yyyy-MM-ddTHH:mm:ssZ",
               CultureInfo.InvariantCulture);
            result = TimeZoneInfo.ConvertTimeToUtc(result);
            return result;
        }
    }
}