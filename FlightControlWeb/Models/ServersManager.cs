using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class ServersMamager : IServerManager
    {
        private readonly List<Server> serversList = new List<Server>();

        public IEnumerable<Server> GetAllServers()
        {
            return serversList;
        }

        public void AddServer(Server server)
        {
            serversList.Add(server);
        }

        public Server GetServerById(string id)
        {
            Server server = serversList.Where(x => x.ServerId == id).FirstOrDefault();
            if (server == null)
            {
                Debug.WriteLine("server isn't found.\n");
                return null;
            }
            else
            {
                return server;
            }
        }

        public Server RemoveServer(string id)
        {
            Server server = serversList.Where(x => x.ServerId == id).FirstOrDefault();
            if (server == null)
            {
                Debug.WriteLine("server isn't found.\n");
                return null;
            }
            else
            {
                serversList.Remove(server);
                return server;
            }
        }
    }
}
