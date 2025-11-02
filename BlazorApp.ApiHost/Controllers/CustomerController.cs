using BlazorApp.ApiHost.Common;
using BlazorApp.Application.Interfaces.Services.Customer;
using BlazorApp.Contracts.Api.Input;
using BlazorApp.Contracts.Api.Output.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using BlazorApp.Application.Mapping;
using BlazorApp.ApiHost.Mapping.RequestMapping;
using BlazorApp.ApiHost.Mapping.DtoMapping;

namespace BlazorApp.ApiHost.Controllers;

[ApiController]
[Route(RouteConstants.V1.CustomerController)]
public sealed class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    [Route(RouteConstants.V1.Heartbeat)]
    public IActionResult Heartbeat()
    {
        return Ok("tick");
    }

    [HttpPost]
    [Route(RouteConstants.V1.UpsertCustomer)]
    public async Task<IActionResult> UpsertCustomerAsync(UpsertCustomerRequest request, CancellationToken cancellationToken)
    {
        var serviceResponse = await customerService.UpsertCustomerAsync(request.ToDomain(), cancellationToken);

        return serviceResponse.Error is null
            ? Ok(ResponseCreator.GetSuccessResponse(serviceResponse.Model.ToDto()))
            : BadRequest(ResponseCreator.GetFailureResponse(serviceResponse.Error.ToResponseResultDto()));
    }

    [HttpGet]
    [Route(RouteConstants.V1.GetCustomer)]
    public async Task<IActionResult> GetCustomerAsync(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        var serviceResponse = await customerService.GetCustomerAsync(request.ToDomain(), cancellationToken);
        
        return serviceResponse.Error is null
            ? Ok(ResponseCreator.GetSuccessResponse(serviceResponse.Model?.ToDto()))
            : serviceResponse.Error.Code == Domain.ErrorCode.NotFound
                ? Ok(ResponseCreator.GetPartialSuccessResponse(serviceResponse.Model, serviceResponse.Error.ToResponseResultDto()))
                : BadRequest(ResponseCreator.GetFailureResponse(serviceResponse.Error.ToResponseResultDto()));
    }

    [HttpGet]
    [Route(RouteConstants.V1.GetCustomers)]
    public async Task<IActionResult> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellation)
    {
        var serviceResponse = await customerService.GetCustomersAsync(request.ToDomain(), cancellation);

        return serviceResponse.Error is null
            ? Ok(ResponseCreator.GetSuccessResponse(serviceResponse.Model.ToDto()))
            : BadRequest(ResponseCreator.GetFailureResponse(serviceResponse.Error.ToResponseResultDto()));
    }

    [HttpPost]
    [Route(RouteConstants.V1.DeleteCustomer)]
    public async Task<IActionResult> DeleteCustomerAsync([FromBody]DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var serviceResponse = await customerService.DeleteCustomerAsync(request.ToDomain(), cancellationToken);

        return serviceResponse.Error is null ? Ok() : BadRequest();
    }
}
