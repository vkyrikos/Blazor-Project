using System.Text.Json.Serialization;

namespace BlazorApp.Contracts.Api.Output.Response;

public sealed class ResponseDto<T> : ResponseDto where T : class
{
    public T Data { get; init; }

    [JsonConstructor]
    public ResponseDto()
    {
        
    }
}

public class ResponseDto
{
    public ResponseCodeDto ResponseCode { get; init; }

    public ResponseResultDto ResponseResult { get; init; }

    [JsonConstructor]
    public ResponseDto()
    {
        
    }
}
