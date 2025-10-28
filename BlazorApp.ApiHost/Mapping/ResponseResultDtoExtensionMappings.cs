using BlazorApp.Application.Interfaces.Validation;
using BlazorApp.Contracts.Api.Output.Response;
using BlazorApp.Domain;

namespace BlazorApp.ApiHost.Mapping;

internal static class ResponseResultDtoExtensionMappings
{
    internal static ResponseResultTypeDto ToResponseResultTypeDto(this ErrorCode errorCode) => errorCode switch
    {
        ErrorCode.Generic => ResponseResultTypeDto.Technical,
        ErrorCode.Validation => ResponseResultTypeDto.Validation,
        ErrorCode.NotFound => ResponseResultTypeDto.Business,
        _ => ResponseResultTypeDto.Technical
    };

    internal static ResponseResultDto ToResponseResultDto(this IError error)
    {
        if (error is null)
        {
            return new();
        }

        return new ResponseResultDto
        {
            Code = error.Message,
            Type = error.Code.ToResponseResultTypeDto(),
        };
    }
}
