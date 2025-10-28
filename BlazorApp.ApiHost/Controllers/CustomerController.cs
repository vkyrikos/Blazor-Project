using BlazorApp.ApiHost.Common;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Contracts.Api.Input;
using BlazorApp.Contracts.Api.Output.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
        var serviceResponse = await customerService.GetCustomerAsync(request.Id);
        return HandleEndpointResponse(serviceResponse);
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
        //await customerService.DeleteCustomerAsync(id);

        return Ok("deleted");
    }

    private IActionResult HandleEndpointResponse(Application.Interfaces.Common.IServiceResponse<Domain.Models.Customer> serviceResponse)
    {
        return serviceResponse.Error is null ? Ok(ResponseCreator.GetSuccessResponse(serviceResponse)) :
            serviceResponse.Error.Code == Domain.ErrorCode.NotFound ? NoContent()
            : BadRequest(ResponseCreator.GetFailureResponse(serviceResponse.Error.ToResponseResultDto()));
    }
}
