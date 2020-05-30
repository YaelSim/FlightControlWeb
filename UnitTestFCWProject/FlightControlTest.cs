using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using System.Collections.Generic;

namespace UnitTestFCWProject
{
    [TestClass]
    public class FlightControlTest
    {
        [TestMethod]
        public void CheckRemoveServer()
        {
            // Create servers list
            IServerManager serversList = new ServersMamager();
            Server serverA = new Server { ServerId = "1234", ServerURL = "AAA1234" };
            Server serverB = new Server { ServerId = "5678", ServerURL = "BBB5678" };
            Server serverC = new Server { ServerId = "9101", ServerURL = "CCC9101" };
            serversList.AddServer(serverA);
            serversList.AddServer(serverB);
            serversList.AddServer(serverC);

            // What the methot need to return
            Server expected = new Server { ServerId = "5678", ServerURL = "BBB5678" };

            //Remove serverB
            Server actual = serversList.RemoveServer("5678");

            // Test
            Assert.AreEqual(expected.ServerId, actual.ServerId);
            Assert.AreEqual(expected.ServerURL, actual.ServerURL);
        }
    }
}
