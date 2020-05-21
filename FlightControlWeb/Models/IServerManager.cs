using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IServerManager
    {
        public IEnumerable<Server> GetAllServers();
        public void AddServer(Server server);
        public Server GetServerById(string id);
        public void RemoveServer(string id);
    }
}
