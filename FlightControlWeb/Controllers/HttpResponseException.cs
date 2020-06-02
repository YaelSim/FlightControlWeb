using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{
    //This class enables creating a custom exception.
    //If an error occurs, an instance of this class will be passed to the client side,
    // there its' content will be printed "beautifuly" on the user's screen.
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }
    }
}
