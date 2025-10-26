using BlazorApp.ApiHost.Common;
using BlazorApp.Contracts.Api.Input;
using BlazorApp.Contracts.Api.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BlazorApp.ApiHost.Controllers;

[ApiController]
public sealed class CustomerController : ControllerBase
{
    [HttpGet]
    [Route("heartbeat")]
    public IActionResult Heartbeat()
    {
        return Ok("heartbeat");
    }

    [HttpGet]
    [Route(RouteConstants.V1.GetCustomer)]
    public IActionResult GetCustomer(GetCustomerRequest request)
    {
        return Ok(new CustomerDto
        {
            Id = request.Id
        });
    }
}
