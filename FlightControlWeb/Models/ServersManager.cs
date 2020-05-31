using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache cache;

        public ServersMamager(IMemoryCache c)
        {
            this.cache = c;
        }

        public IEnumerable<Server> GetAllServers()
        {
            var fromCache = ((IEnumerable<Server>)cache.Get("serversList")).ToList();
            //             cache.Set("serversList", fromCache);
            return serversList;
        }

        public void AddServer(Server server)
        {
            var fromCache = ((IEnumerable<Server>)cache.Get("serversList")).ToList();
            fromCache.Add(server);
            cache.Set("serversList", fromCache);
            serversList.Add(server);
        }

        public Server GetServerById(string id)
        {
            var fromCache = ((IEnumerable<Server>)cache.Get("serversList")).ToList();

            Server server = fromCache.Where(x => x.ServerId == id).FirstOrDefault();
            //Server server = serversList.Where(x => x.ServerId == id).FirstOrDefault();
            cache.Set("servers", fromCache);

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
            var fromCache = ((IEnumerable<Server>)cache.Get("serversList")).ToList();

            //Server server = serversList.Where(x => x.ServerId == id).FirstOrDefault();
            Server server = fromCache.Where(x => x.ServerId == id).FirstOrDefault();
            cache.Set("serversList", fromCache);
            if (server == null)
            {
                Debug.WriteLine("server isn't found.\n");
                return null;
            }
            else
            {
                //serversList.Remove(server);
                fromCache.Remove(server);
                return server;
            }
        }
    }
}
