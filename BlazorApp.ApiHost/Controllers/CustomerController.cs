using BlazorApp.ApiHost.Common;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Contracts.Api.Input;
using BlazorApp.Contracts.Api.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace BlazorApp.ApiHost.Controllers;

[ApiController]
public sealed class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    [Route(RouteConstants.V1.Heartbeat)]
    public IActionResult Heartbeat()
    {
        return Ok("tick");
    }

    [HttpGet]
    [Route(RouteConstants.V1.GetCustomer)]
    public async Task<IActionResult> GetCustomerAsync(GetCustomerRequest request)
    {
        var result = await customerService.GetCustomerAsync(request.Id);

        return Ok(new CustomerDto
        {
            Id = request.Id
        });
    }

    [HttpPost]
    [Route(RouteConstants.V1.UpsertCustomer)]
    public async Task<IActionResult> UpsertCustomerAsync(UpsertCustomerRequest request)
    {
        //TODO: Add upsert service method.
        //await customerService.UpsertCustomerAsync();

        return Ok("upserted");
    }

    [HttpPost]
    [Route(RouteConstants.V1.DeleteCustomer)]
    public async Task<IActionResult> DeleteCustomerAsync([FromRoute]string id)
    {
        await customerService.DeleteCustomerAsync(id);

        return Ok("deleted");
    }
}
