using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        //private readonly ServersMamager service = new ServersMamager();
        private readonly IMemoryCache cache;
        private readonly IServerManager service;

        public ServersController(IMemoryCache c)
        {
            cache = c;
            service = new ServersMamager(cache);
        }

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
            if ((s == null) || (s.ServerId == null) || (s.ServerURL == null))
            {
                HttpResponseException hre = new HttpResponseException
                {
                    Status = 400,
                    Value = "Server Cannot Be Added - Metadata Doesn't Meet Concerns."
                };
                throw hre;
            }
            service.AddServer(s);
            return s;
        }

        //DELETE: /api/servers/{id}
        [HttpDelete("{id}")]
        public ActionResult<Server> RemoveServer(string id)
        {
            Server found = service.RemoveServer(id);
            if (found == null)
            {
                HttpResponseException hre = new HttpResponseException
                {
                    Status = 404,
                    Value = "Server Cannot Be Removed, Since It Was Not Found"
                };
                throw hre;
            } else
            {
                return found;
            }
        }
    }
}