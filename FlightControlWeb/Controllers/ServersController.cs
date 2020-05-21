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
    public class ServersController : ControllerBase
    {
        private readonly ServersMamager service = new ServersMamager();

        //GET: /api/servers
        [HttpGet]
        public IEnumerable<Server> GetAllServers()
        {
            return service.GetAllServers();
        }

        //POST: /api/servers
        [HttpPost]
        public Server AddServer([FromBody] Server s)
        {
            service.AddServer(s);
            return s;
        }

        //DELETE: /api/servers/{id}
        [HttpDelete("{id}")]
        public void RemoveServer(string id)
        {
            service.RemoveServer(id);
        }
    }
}