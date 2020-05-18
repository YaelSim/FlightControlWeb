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
        private FlightPlanManager service;

        // GET: /api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public IEnumerable<Flight> GetInternalFlights()
        {
            // initialize the service
            GetService();
            string dateTime = Request.QueryString.Value;
            DateTime result = GetDateTimeAccordingfToStr(dateTime);
            return service.GetInternalFlightsRelative(result);
        }

        // GET: /api/Flights?relative_to=<DATE_TIME>&sync_all
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights()
        {
            // initialize the service
            GetService();
            string dateTime = Request.QueryString.Value;
            DateTime result = GetDateTimeAccordingfToStr(dateTime);
            return service.GetAllFlightsRelative(result);
        }

        // DELETE: /api/Flights/{id}
        [HttpDelete]
        public void DeleteFlightPlan(string id)
        {
            // initialize the service
            GetService();
            service.RemoveFlightPlan(id);
        }

        private DateTime GetDateTimeAccordingfToStr(string dateTime)
        {
            DateTime result = DateTime.ParseExact(dateTime, "yyyy-MM-ddTHH:mm:ssZ",
               CultureInfo.InvariantCulture);
            result = TimeZoneInfo.ConvertTimeToUtc(result);
            return result;
        }

        private void GetService()
        {
            var allServices = this.HttpContext.RequestServices;
            this.service = (FlightPlanManager)allServices.GetService(typeof(FlightPlanManager));
        }
    }
}