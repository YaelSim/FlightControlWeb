using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = exception.Status,
                };
                context.ExceptionHandled = true;
            }
            /*if (context.Exception is ArgumentNullException)
            {
                context.Result = new BadRequestObjectResult("Bad Request. Try Again.");
                context.ExceptionHandled = true;
            }
            if (context.Exception is ArgumentOutOfRangeException)
            {
                context.Result = new BadRequestObjectResult(
                    "Given Information is Out of Range. Try Again.");
                context.ExceptionHandled = true;
            }
            if (context.Exception is FormatException)
            {
                context.Result = new ObjectResult("Wrong DateTime Format. Try Again.");
                context.ExceptionHandled = true;
            }
            if (context.Exception is InvalidOperationException)
            {
                context.Result = new BadRequestObjectResult(
                    "Bad Request - Data Does Not Exist In Current Context");
                context.ExceptionHandled = true;
            }*/
        }
    }
}
