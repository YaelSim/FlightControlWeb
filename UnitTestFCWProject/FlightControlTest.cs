using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace UnitTestFCWProject
{
    [TestClass]
    public class FlightControlTest
    {
        // This method checks if the flight is active in the given date time.
        [TestMethod]
        public void CheckIfFlightActive ()
        {
            // Create Flight plans
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            IServerManager servers = new ServersMamager(cache);
            FlightPlanManager flightPlanManager = new FlightPlanManager(servers);

            // time of the flight
            string exampleA = "2020-05-31T00:00:00Z";
            DateTime timeA = DateTime.ParseExact(exampleA, "yyyy-MM-ddTHH:mm:ssZ",
                System.Globalization.CultureInfo.InvariantCulture);
            timeA = TimeZoneInfo.ConvertTimeToUtc(timeA);

            List<Segment> segments = new List<Segment>
            {
                new Segment
                {
                    Longitude = 32,
                    Latitude = 32,
                    TimespanSeconds = 20
                },
                new Segment
                {
                    Longitude = 38,
                    Latitude = 38,
                    TimespanSeconds = 40
                }
            };
            InitialLocation initial = new InitialLocation { 
                Longitude = 32,
                Latitude = 32,
                DateTime = timeA
            };
            FlightPlan flight = new FlightPlan {
                Passengers = 200,
                CompanyName = "ISRAIR",
                InitialLocation = initial,
                Segments = segments
            };

            // time after flight
            string exampleB = "2020-05-31T00:01:10Z";
            DateTime timeB = DateTime.ParseExact(exampleB, "yyyy-MM-ddTHH:mm:ssZ",
                System.Globalization.CultureInfo.InvariantCulture);
            timeB = TimeZoneInfo.ConvertTimeToUtc(timeB);

            // What the methot need to return
            bool expectedA = true;
            bool expectedB = false;

            //check id flight is active
            bool actualA = flightPlanManager.IsFlightActive(flight, timeA);
            bool actualB = flightPlanManager.IsFlightActive(flight, timeB);

            // Test
            Assert.AreEqual(expectedA, actualA);
            Assert.AreEqual(expectedB, actualB);
        }

        // This method checks the total flight time by segments list.
        [TestMethod]
        public void CheckTotalTimeOfFlight()
        {
            // Create Segments list
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            IServerManager servers = new ServersMamager(cache);
            FlightPlanManager flightPlanManager = new FlightPlanManager(servers);
            List<Segment> segments = new List<Segment>
            {
                new Segment
                {
                    Longitude = 32,
                    Latitude = 62,
                    TimespanSeconds = 13
                },
                new Segment
                {
                    Longitude = 32,
                    Latitude = 62,
                    TimespanSeconds = 15
                },
                  new Segment
                {
                    Longitude = 32,
                    Latitude = 62,
                    TimespanSeconds = 26
                },
                new Segment
                {
                    Longitude = 32,
                    Latitude = 62,
                    TimespanSeconds = 167
                }
            };

            // What the methot need to return
            double expected = 221;

            // Calculate total time of the flight
            double actual = flightPlanManager.GetTotalTimeOfFlight(segments);

            // Test
            Assert.AreEqual(expected, actual);
        }

        // This method checks witch segment of the flightws path it is now.
        [TestMethod]
        public void CheckFlightCurrentSegment()
        {
            //* Create Segments list
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            IServerManager servers = new ServersMamager(cache);
            FlightPlanManager flightPlanManager = new FlightPlanManager(servers);
            List<Segment> segments = new List<Segment>
            {
                new Segment
                {
                    Longitude = 32,
                    Latitude = 62,
                    TimespanSeconds = 10
                },
                new Segment
                {
                    Longitude = 34,
                    Latitude = 64,
                    TimespanSeconds = 15
                },
                  new Segment
                {
                    Longitude = 36,
                    Latitude = 66,
                    TimespanSeconds = 20
                },
                new Segment
                {
                    Longitude = 38,
                    Latitude = 68,
                    TimespanSeconds = 25
                },
                   new Segment
                {
                    Longitude = 39,
                    Latitude = 69,
                    TimespanSeconds = 30
                },
                new Segment
                {
                    Longitude = 37,
                    Latitude = 67,
                    TimespanSeconds = 35
                }
            };
            double totalTime = 50;

            // What the methot need to return
            int expected = 3;

            // Get the index of the segment in the list
            int actual = flightPlanManager.GetFlightCurrentSegment(segments, totalTime);

            // Test
            Assert.AreEqual(expected, actual);
        }

        // This method checks if the right server was removed from the list.
        [TestMethod]
        public void CheckRemoveServer()
        {
            // Create servers list
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            List<Server> serversList = new List<Server>();
            Server serverA = new Server { ServerId = "1234", ServerURL = "AAA1234" };
            Server serverB = new Server { ServerId = "5678", ServerURL = "BBB5678" };
            Server serverC = new Server { ServerId = "9101", ServerURL = "CCC9101" };
            serversList.Add(serverA);
            serversList.Add(serverB);
            serversList.Add(serverC);

            cache.Set("serversList", serversList);
            IServerManager servers = new ServersMamager(cache);

            // What the methot need to return
            Server expected = new Server { ServerId = "5678", ServerURL = "BBB5678" };

            //Remove serverB
            Server actual = servers.RemoveServer("5678");

            // Test
            Assert.AreEqual(expected.ServerId, actual.ServerId);
            Assert.AreEqual(expected.ServerURL, actual.ServerURL);
        }

        // This method checks if the ServerManager returns all the list completle.
        [TestMethod]
        public void CheckReturnedServersList()
        {
            // Create servers list
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            List<Server> serversList = new List<Server>();
            Server serverA = new Server { ServerId = "1234", ServerURL = "AAA1234" };
            Server serverB = new Server { ServerId = "5678", ServerURL = "BBB5678" };
            Server serverC = new Server { ServerId = "9101", ServerURL = "CCC9101" };
            serversList.Add(serverA);
            serversList.Add(serverB);
            serversList.Add(serverC);

            cache.Set("serversList", serversList);
            IServerManager servers = new ServersMamager(cache);

            // What the methot need to return
            int expectedCount = 3;
            List<Server> expectedList = serversList;

            //Get list of all the servers
            IEnumerable<Server> actual = servers.GetAllServers();

            // Test
            Assert.AreEqual(expectedCount, actual.Count());
            int i = 0;
            foreach (Server server in actual)
            {
                Assert.AreEqual(expectedList[i].ServerId, server.ServerId);
                Assert.AreEqual(expectedList[i].ServerURL, server.ServerURL);
                i++;
            }
        }
    }
}
